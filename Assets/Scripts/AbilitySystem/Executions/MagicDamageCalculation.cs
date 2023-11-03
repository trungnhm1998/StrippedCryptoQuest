using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Executions
{
    public class MagicDamageCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseMagicAttack;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref GameplayEffectCustomExecutionOutput outModifiers)
        {
            var spec = executionParams.EffectSpec;
            var context = GameplayEffectContext.ExtractEffectContext(spec.Context);
            if (context == null)
            {
                Debug.LogWarning("MagicDamageCalculation::Execute::context is null" +
                                 "\nThis execution depends on DerivedGameplayEffectContext");
                return;
            }

            SkillParameters skillParameters = context.SkillInfo.SkillParameters;
            CustomExecutionAttributeCaptureDef targetAttribute = skillParameters.targetAttribute;
            var elementalRate = BattleCalculator.CalculateElementalRateFromParams(executionParams);
            var effectType = skillParameters.EffectType;
            executionParams.TryGetAttributeValue(_baseMagicAttack, out var baseAttack);
            float damageValue = 0;
            if (skillParameters.IsFixed)
            {
                float skillPower = BattleCalculator.CalculateMagicSkillBasePower(skillParameters,
                    baseAttack.CurrentValue);
                skillPower.Offset(Random.Range(_lowerRandomRange, _upperRandomRange));
                damageValue = effectType == EEffectType.RemoveAbnormalStatus
                    ? skillPower
                    : skillPower * elementalRate;
                damageValue = Mathf.RoundToInt(damageValue);
            }
            else
            {
                float percentage = BattleCalculator.CalculateMagicSkillBasePower(skillParameters,
                    baseAttack.CurrentValue);
                executionParams.TryGetAttributeValue(targetAttribute, out var targetedAttributeValue);
                damageValue = targetedAttributeValue.CurrentValue *
                              (percentage / BaseBattleVariable.CORRECTION_PROBABILITY_VALUE);
            }
            
            var mod = BattleCalculator.GetEffectTypeValueCorrection(effectType);

            Debug.Log("magic damage dealt: " + damageValue);
            if (damageValue <= 0f) return;
            var modifier = new GameplayModifierEvaluatedData()
            {
                Attribute = targetAttribute.Attribute,
                OpType = EAttributeModifierOperationType.Add,
                Magnitude = Mathf.RoundToInt(damageValue) * mod
            };
            outModifiers.Add(modifier);
        }
    }
}