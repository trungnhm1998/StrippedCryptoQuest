using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    public class ClampAttribute : AttributesEventBase
    {
        [SerializeField] private CoreAttribute _attribute;
        [SerializeField] private CoreAttribute _maxAttribute;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue)
        {
            if (_attribute != newAttributeValue.Attribute) return;
            var cacheDict = attributeSystem.GetAttributeIndexCache();
            ClampAttributeToMax(_attribute, _maxAttribute, ref newAttributeValue, cacheDict, attributeSystem);
        }

        private void ClampAttributeToMax(CoreAttribute attribute,
            CoreAttribute maxAttribute,
            ref AttributeValue newAttributeValue,
            Dictionary<CoreAttribute, int> cacheDict, AttributeSystemBehaviour attributeSystemBehaviour)
        {
            if (!cacheDict.TryGetValue(attribute, out int attributeIdx)
                || !cacheDict.TryGetValue(_maxAttribute, out int maxAttributeIdx))
            {
                Debug.LogWarning($"Try to clamp attribute {attribute.name} to max attribute {maxAttribute.name} " +
                                 $"but one or both of them are not in the attribute system.");
                return;
            }

            var maxAttributeValue = attributeSystemBehaviour.AttributeValues[maxAttributeIdx];
            var preChange = newAttributeValue.Clone();

            if (newAttributeValue.CurrentValue > maxAttributeValue.CurrentValue)
            {
                newAttributeValue.CurrentValue = maxAttributeValue.CurrentValue;
                Debug.Log(
                    $"Clamped current attribute {attribute.name} from [{preChange.CurrentValue}] to [{newAttributeValue.CurrentValue}]");
            }

            if (newAttributeValue.BaseValue > maxAttributeValue.BaseValue)
            {
                newAttributeValue.BaseValue = maxAttributeValue.BaseValue;
                Debug.Log(
                    $"Clamped base {maxAttribute.name} from [{preChange.BaseValue}] to [{newAttributeValue.BaseValue}].");
            }
        }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) { }
    }
}