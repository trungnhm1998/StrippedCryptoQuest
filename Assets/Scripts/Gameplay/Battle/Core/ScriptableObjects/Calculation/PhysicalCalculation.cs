using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    public class PhysicalCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseAttack;
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseDefense;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            CryptoQuestGameplayEffectSpec effectSpec = (CryptoQuestGameplayEffectSpec)executionParams.EffectSpec;
            SkillParameters skillParameters = effectSpec.SkillParam;
            var targetedAttributeSO = skillParameters.targetAttribute;
            var effectType = skillParameters.EffectType;
            var elementalRate = BattleCalculator.CalculateElementalRateFromParams(executionParams);
            executionParams.TryGetAttributeValue(_baseAttack, out var baseAttack);
            executionParams.TryGetAttributeValue(_baseDefense, out var baseDefense);

            float baseDamageValue = BattleCalculator.CalculateBaseDamage(skillParameters, baseAttack.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));
            float damageValue = 0;
            if (skillParameters.IsFixed)
                damageValue = BattleCalculator.CalculateFixedPhysicalDamage(baseDamageValue, elementalRate);
            else
                damageValue =
                    BattleCalculator.CalculatePercentPhysicalDamage(baseDamageValue, baseAttack.CurrentValue,
                        baseDefense.CurrentValue, elementalRate);
            var mod = BattleCalculator.GetEffectTypeValueCorrection(effectType);

            if (baseDamageValue > 0f)
            {
                var modifier = new EffectAttributeModifier()
                {
                    Attribute = targetedAttributeSO.Attribute,
                    ModifierType = EAttributeModifierType.Add,
                    Value = damageValue * mod
                };
                outModifiers.Add(modifier);
            }
        }
    }
}