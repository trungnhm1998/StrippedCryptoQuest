using IndiGames.GameplayAbilitySystem.AttributeSystem;

namespace IndiGames.GameplayAbilitySystem.Helper
{
    public static class AttributeSystemHelper
    {
        public static AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue)
        {
            float overridingValue = attributeValue.ExternalModifier.Overriding + attributeValue.CoreModifier.Overriding;
            if (overridingValue != 0)
            {
                attributeValue.CurrentValue = overridingValue;
                return attributeValue;
            }

            var coreValue = CalculateCoreAttributeValue(attributeValue);

            attributeValue.CurrentValue = (coreValue + attributeValue.ExternalModifier.Additive) *
                                          (attributeValue.ExternalModifier.Multiplicative + 1);
            return attributeValue;
        }

        public static float CalculateCoreAttributeValue(AttributeValue attributeValue)
        {
            var coreModifier = attributeValue.CoreModifier;

            if (coreModifier.Overriding != 0)
            {
                return coreModifier.Overriding;
            }
            return (attributeValue.BaseValue + coreModifier.Additive) *
                (coreModifier.Multiplicative + 1);
        }
    }
}
