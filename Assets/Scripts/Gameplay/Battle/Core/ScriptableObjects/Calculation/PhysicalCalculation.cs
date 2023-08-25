using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
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
            var mod = 1f;
            switch (effectType)
            {
                case EEffectType.Damage:
                case EEffectType.DeBuff:
                    mod = -1f;
                    break;
                default:
                    mod = 1f;
                    break;
            }

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