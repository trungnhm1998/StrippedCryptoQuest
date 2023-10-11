using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.ModifierComputationStrategies;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Abilities
{
    public class MaxAttributePercentageRecoveryComputation : ModifierComputationSO
    {
        [SerializeField, Range(0, 1)] private float _multiplier = 1f;
        [SerializeField] private AttributeScriptableObject _maxAttribute;
        public override void Initialize(GameplayEffectSpec effectSpec) { }

        public override bool AttemptCalculateMagnitude(GameplayEffectSpec effectSpec,
            ref float evaluatedMagnitude)
        {
            effectSpec.Target.AttributeSystem.TryGetAttributeValue(_maxAttribute, out var maxAttributeValue);
            if (maxAttributeValue.CurrentValue.NearlyEqual(0f)) return false;

            evaluatedMagnitude = maxAttributeValue.CurrentValue * _multiplier;
            Debug.Log(
                $"Calculated magnitude using [{_maxAttribute}] with value [{maxAttributeValue.CurrentValue}]: {evaluatedMagnitude} for {effectSpec.Target}");
            return true;
        }
    }
}