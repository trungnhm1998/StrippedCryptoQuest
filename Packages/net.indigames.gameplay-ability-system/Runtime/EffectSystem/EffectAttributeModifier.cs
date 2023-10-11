using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.ModifierComputationStrategies;
using JetBrains.Annotations;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem
{
    /// <summary>
    /// Define how the effect will modify an attribute
    /// </summary>
    [Serializable]
    public struct EffectAttributeModifier
    {
        [Tooltip("Select which attribute will be affected by this modifier")]
        public AttributeScriptableObject Attribute;

        [Tooltip("Select which type of modifier")]
        public EAttributeModifierOperationType OperationType;

        /// <summary>
        /// Magnitude of this modifier
        /// </summary>
        [Tooltip("How the Magnitude of this modifier will be calculated")]
        [CanBeNull]
        public ModifierComputationSO ModifierMagnitude;

        [Tooltip("Effect value")]
        public float Value;

        public EffectAttributeModifier Clone()
        {
            return new EffectAttributeModifier
            {
                Attribute = Attribute,
                OperationType = OperationType,
                ModifierMagnitude = ModifierMagnitude,
                Value = Value
            };
        }
    }

    public enum EAttributeModifierOperationType
    {
        Add = 0,
        Multiply = 1,
        Division = 2,
        Override = 3
    }
}