using System.Collections.Generic;
using CryptoQuest.Character.Ability;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.EffectExecutionCalculation;
using UnityEngine;

namespace CryptoQuest.Battle.EffectCalculations
{
    [CreateAssetMenu(fileName = "HealCalculation",
        menuName = "Gameplay/Battle/Effects/Execution Calculations/Heal Calculation")]
    public class MagicCalculation : EffectExecutionCalculationBase
    {
        [SerializeField] private CustomExecutionAttributeCaptureDef _baseMagicAttack;
        [SerializeField] private float _lowerRandomRange = -0.05f;
        [SerializeField] private float _upperRandomRange = 0.05f;

        public override void Execute(ref CustomExecutionParameters executionParams,
            ref List<EffectAttributeModifier> outModifiers)
        {
            var effectSpec = (EffectSpec)executionParams.EffectSpec;
            executionParams.TryGetAttributeValue(_baseMagicAttack, out var baseMagicAttack);
            var targetedAttributeDef = effectSpec.Parameters.targetAttribute;
            float baseMagicValue = BattleCalculator.CalculateBaseDamage(effectSpec.Parameters,
                baseMagicAttack.CurrentValue,
                Random.Range(_lowerRandomRange, _upperRandomRange));

            Debug.Log($"Magic value: {baseMagicAttack}");

            if (baseMagicValue <= 0f) return;

            var modifier = new EffectAttributeModifier()
            {
                Attribute = targetedAttributeDef.Attribute,
                ModifierType = EAttributeModifierType.Add,
                // TODO: Refactor this when implement buff effect
                Value = Mathf.RoundToInt(baseMagicValue) * (effectSpec.Parameters.EffectType == EEffectType.Restore ? 1 : -1)
            };
            outModifiers.Add(modifier);
        }
    }
}