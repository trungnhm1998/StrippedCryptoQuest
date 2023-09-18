using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.ModifierComputationStrategies
{
    /// <summary>
    /// - Scalable Float
    ///     Use Level the scale the value
    /// - Attribute Based
    ///     Take CurrentValue or BaseValue of a backing Attribute on the Source (Who created the GameplayEffectSpec)
    ///     or the Target (Who received the GameplayEffectSpec) and further modify it by a coefficient.
    /// - Custom Calculation Class
    ///     <see cref="DefaultFloatComputation"/>
    /// - Set By Caller
    ///     Imagine player holding a button 
    /// </summary>
    public abstract class ModifierComputationSO : ScriptableObject
    {
        /// <summary>
        /// Called when the spec is first initialised
        /// </summary>
        /// <param name="effectSpec">Gameplay Effect Spec</param>
        public abstract void Initialize(GameplayEffectSpec effectSpec);

        /// <summary>
        /// Function called when the magnitude is calculated, usually after the target has been assigned
        /// </summary>
        /// <param name="effectSpec">Gameplay Effect Spec</param>
        /// <returns></returns>
        public abstract float? CalculateMagnitude(GameplayEffectSpec effectSpec);

        public virtual bool AttemptCalculateMagnitude(GameplayEffectSpec gameplayEffectSpec,
            out float evaluatedMagnitude)
        {
            throw new System.NotImplementedException();
        }
    }
}