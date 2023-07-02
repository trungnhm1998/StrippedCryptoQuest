using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem
{
    [Serializable]
    public class AttributeValue
    {
        public AttributeScriptableObject Attribute;
        public float BaseValue;
        public float CurrentValue;
        /// <summary>
        ///For ability/effect external stats
        /// </summary>
        public Modifier Modifier = new Modifier();
        public Modifier CoreModifier = new Modifier();

        public AttributeValue()
        {
        }

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
                Modifier =  Modifier,
                CoreModifier = CoreModifier,
                Attribute = Attribute
            };
        }
    }
}