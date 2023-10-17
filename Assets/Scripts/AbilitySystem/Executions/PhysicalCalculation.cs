using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class PhysicalCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseAttack;
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseDefense;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            if (context == null)
            {
                Debug.LogWarning("PhysicalCalculation::Execute::context is null" +
                                 "\nThis execution depends on DerivedGameplayEffectContext");
                return;
            }

            SkillParameters skillParameters = context.SkillInfo.SkillParameters;
            var targetedAttributeSO = skillParameters.targetAttribute;
            var effectType = skillParameters.EffectType;
            var elementalRate = BattleCalculator.CalculateElementalRateFromParams(executionParams);
            executionParams.TryGetAttributeValue(_baseAttack, out var baseAttack);
            executionParams.TryGetAttributeValue(_baseDefense, out var baseDefense);

            float baseDamageValue = BattleCalculator.CalculateBaseDamage(skillParameters, baseAttack.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));
            float damageValue = 0;
            if (skillParameters.IsFixed)
                damageValue =
                    Mathf.RoundToInt(BattleCalculator.CalculateFixedPhysicalDamage(baseDamageValue, elementalRate));
            else
                damageValue =
                    BattleCalculator.CalculatePercentPhysicalDamage(baseDamageValue, baseAttack.CurrentValue,
                        baseDefense.CurrentValue, elementalRate);
            var mod = BattleCalculator.GetEffectTypeValueCorrection(effectType);

            if (damageValue <= 0f) return;

            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = targetedAttributeSO.Attribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = damageValue * mod
            };
            outModifiers.Add(modifier);
        }
    }
}