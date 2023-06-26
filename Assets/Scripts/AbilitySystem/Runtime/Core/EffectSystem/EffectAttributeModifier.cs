using System;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [Serializable]
    public struct EffectAttributeModifier
    {
        [Tooltip("Select which attribute will be affected by this modifier")]
        public AttributeScriptableObject AttributeSO;
        [Tooltip("Select which type of modifier")]
        public EAttributeModifierType ModifierType;
        [Tooltip("How the modifier will be computate")]
        public ModifierComputationSO ModifierComputationMethod;
        [Tooltip("Effect value")]
        public float Value;
        
        public EffectAttributeModifier Clone()
        {
            return new EffectAttributeModifier
            {
                AttributeSO = AttributeSO,
                ModifierType = ModifierType,
                ModifierComputationMethod = ModifierComputationMethod,
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