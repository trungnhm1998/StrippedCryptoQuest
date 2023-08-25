using System;
using System.Collections.Generic;
using CryptoQuest.Character.Attributes;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Calculation
{
    public class MagicDamageCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseMagicAttack;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            CryptoQuestGameplayEffectSpec effectSpec = (CryptoQuestGameplayEffectSpec)executionParams.EffectSpec;
            SkillParameters skillParameters = effectSpec.SkillParam;
            CustomExecutionAttributeCaptureDef targetAttribute = skillParameters.targetAttribute;
            var elementalRate = BattleCalculator.CalculateElementalRateFromParams(executionParams);
            var effectType = skillParameters.EffectType;
            executionParams.TryGetAttributeValue(_baseMagicAttack, out var baseAttack);
            float damageValue = 0;
            if (skillParameters.IsFixed)
            {
                float baseMagicDamageFixedValue = BattleCalculator.CalculateBaseDamage(skillParameters,
                    baseAttack.CurrentValue, Random.Range(_lowerRandomRange, _upperRandomRange));
                damageValue = baseMagicDamageFixedValue * elementalRate;
            }
            else
            {
                float percentage = BattleCalculator.CalculateBaseDamage(skillParameters,
                    baseAttack.CurrentValue, 0);
                executionParams.TryGetAttributeValue(targetAttribute, out var targetedAttributeValue);
                damageValue = targetedAttributeValue.CurrentValue *
                              (percentage / BaseBattleVariable.CORRECTION_PROBABILITY_VALUE);
            }

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

            if (damageValue > 0f)
            {
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
}