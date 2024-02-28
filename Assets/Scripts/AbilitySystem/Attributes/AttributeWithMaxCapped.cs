using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.Attributes
{
    public class AttributeWithMaxCapped : AttributeScriptableObject
    {
        [SerializeField] private AttributeScriptableObject _maxAttributeToCap;
        public AttributeScriptableObject CappedAttribute => _maxAttributeToCap;

        public override AttributeValue CalculateInitialValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValues)
        {
            var maxAttributeValue =
                otherAttributeValues.Find(x =>
                    x.Attribute ==
                    _maxAttributeToCap); // TODO: Use attribute system as a param instead of Find like this

            attributeValue.BaseValue = maxAttributeValue.BaseValue;
            attributeValue.CurrentValue = maxAttributeValue.CurrentValue;

            return attributeValue;
        }
    }
}