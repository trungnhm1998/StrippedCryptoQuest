using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem
{
    public enum EModifierType
    {
        External,
        Core,
    }

    /// <summary>
    /// Represent a value of a <see cref="AttributeScriptableObject"/>
    /// Should have use struct but it messing with my Unit Test
    /// </summary>
    [Serializable]
    public struct AttributeValue
    {
        [field: SerializeField] public AttributeScriptableObject Attribute { get; set; }
        [field: SerializeField] public float BaseValue { get; set; }
        [field: SerializeField] public float CurrentValue { get; set; }

        /// <summary>
        /// Sum of all external effects
        /// For ability/effect external stats
        /// This is for external modifier such as temporary buff, in combat buff
        /// </summary>
        public Modifier ExternalModifier;

        /// <summary>
        /// Based on For Honor GDC talk which will cause wrong calculation
        /// This is for Gameplay Difficulty multiplier, Gear, Permanent Buffs and passive
        /// <seealso herf="https://www.youtube.com/watch?v=JgSvuSaXs3E"/>
        /// </summary>
        public Modifier CoreModifier;

        public AttributeValue(AttributeScriptableObject attribute)
        {
            Attribute = attribute;
            BaseValue = CurrentValue = 0f;
            ExternalModifier = new Modifier();
            CoreModifier = new Modifier();
        }

        public AttributeValue Clone()
        {
            return new AttributeValue()
            {
                CurrentValue = CurrentValue,
                BaseValue = BaseValue,
                ExternalModifier = ExternalModifier,
                CoreModifier = CoreModifier,
                Attribute = Attribute
            };
        }
    }

    public static class FloatExtension
    {
        public static bool NearlyEqual(this float a, float b, float epsilon = 0.001f)
        {
            return Mathf.Abs(a - b) < epsilon;
        }
    }
}