using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.ModifierComputationStrategies;
using UnityEngine;

namespace CryptoQuest.Character.Ability
{
    public class FullRecoveryComputation : ModifierComputationSO
    {
        [SerializeField, Range(0, 1)] private float _multiplier = 1f;
        [SerializeField] private AttributeScriptableObject _maxAttribute;
        public override void Initialize(GameplayEffectSpec effectSpec) { }

        public override float? CalculateMagnitude(GameplayEffectSpec effectSpec)
        {
            effectSpec.Target.AttributeSystem.TryGetAttributeValue(_maxAttribute, out var maxAttributeValue);
            var calculatedMagnitude = maxAttributeValue.CurrentValue * _multiplier;
            Debug.Log($"Calculated magnitude using [{_maxAttribute}] with value [{maxAttributeValue.CurrentValue}]: {calculatedMagnitude} for {effectSpec.Target}");
            return calculatedMagnitude;
        }
    }
}