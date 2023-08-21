using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

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
    public class AttributeValue
    {
        public AttributeScriptableObject Attribute;
        public float BaseValue;
        public float CurrentValue;

        /// <summary>
        /// Sum of all external effects
        /// For ability/effect external stats
        /// This is for external modifier such as temporary buff, in combat buff
        /// </summary>
        public Modifier ExternalModifier = new();

        /// <summary>
        /// Based on For Honor GDC talk which will cause wrong calculation
        /// This is for Gameplay Difficulty multiplier, Gear, Permanent Buffs and passive
        /// <seealso herf="https://www.youtube.com/watch?v=JgSvuSaXs3E"/>
        /// </summary>
        public Modifier CoreModifier = new();

        public AttributeValue() { }

        public AttributeValue(AttributeScriptableObject attribute)
        {
            Attribute = attribute;
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
}