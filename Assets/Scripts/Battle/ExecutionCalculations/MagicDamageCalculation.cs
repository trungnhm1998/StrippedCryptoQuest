using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Battle.ExecutionCalculations
{
    public class MagicDamageCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseMagicAttack;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            var effectSpec = (CQEffectSpec)executionParams.EffectSpec;
            if (effectSpec == null) return;
            SkillParameters skillParameters = effectSpec.Parameters;
            CustomExecutionAttributeCaptureDef targetAttribute = skillParameters.targetAttribute;
            var elementalRate = BattleCalculator.CalculateElementalRateFromParams(executionParams);
            var effectType = skillParameters.EffectType;
            executionParams.TryGetAttributeValue(_baseMagicAttack, out var baseAttack);
            float damageValue = 0;
            if (skillParameters.IsFixed)
            {
                float baseMagicDamageFixedValue = BattleCalculator.CalculateBaseDamage(skillParameters,
                    baseAttack.CurrentValue, Random.Range(_lowerRandomRange, _upperRandomRange));
                damageValue = effectType == EEffectType.RemoveAbnormalStatus
                    ? baseMagicDamageFixedValue
                    : baseMagicDamageFixedValue * elementalRate;
            }
            else
            {
                float percentage = BattleCalculator.CalculateBaseDamage(skillParameters,
                    baseAttack.CurrentValue, 0);
                executionParams.TryGetAttributeValue(targetAttribute, out var targetedAttributeValue);
                damageValue = targetedAttributeValue.CurrentValue *
                              (percentage / BaseBattleVariable.CORRECTION_PROBABILITY_VALUE);
            }
            
            var mod = BattleCalculator.GetEffectTypeValueCorrection(effectType);
            
            Debug.Log("magic damage dealt: " + damageValue * mod);
            if (damageValue <= 0f) return;
            var modifier = new EffectAttributeModifier()
            {
                Attribute = targetAttribute.Attribute,
                ModifierType = EAttributeModifierType.Add,
                Value = damageValue * mod
            };
            outModifiers.Add(modifier);
        }
    }
}