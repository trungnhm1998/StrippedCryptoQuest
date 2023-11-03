using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Attributes
{
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Derived Offensive Attribute")]
    public class DerivedOffensiveAttributeScriptableObject : DerivedAttributeScriptableObject
    {
        public override AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValuesInSystem)
        {
            // get value that is a result of calculation affected by other attributes
            AttributeValue derivedAttributeValue = base.CalculateCurrentAttributeValue(attributeValue, otherAttributeValuesInSystem);

            float cappedCoreModifierMultiplicative = MathF.Max(0.2f, derivedAttributeValue.CoreModifier.Multiplicative + 1);

            float coreValue = (derivedAttributeValue.BaseValue + derivedAttributeValue.CoreModifier.Additive) *
                              cappedCoreModifierMultiplicative;

            float cappedExternalModifierMultiplicative = MathF.Max(0.2f, derivedAttributeValue.ExternalModifier.Multiplicative + 1);

            derivedAttributeValue.CurrentValue = (coreValue + derivedAttributeValue.ExternalModifier.Additive) * 
                                                 cappedExternalModifierMultiplicative;

            return derivedAttributeValue;
        }
    }
}
