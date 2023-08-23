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
        public EAttributeModifierType ModifierType;

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
                ModifierType = ModifierType,
                ModifierMagnitude = ModifierMagnitude,
                Value = Value
            };
        }
    }

    public enum EAttributeModifierType
    {
        Add,
        Multiply,
        Override
    }
}