using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    /// <summary>
    /// Clamp the target attribute to the current value of the max attribute
    ///
    /// example: clamp health to max health or mana to max mana
    /// well the only 2 cases I can think of right now
    /// </summary>
    public class ClampAttribute : AttributesEventBase
    {
        [SerializeField] private CoreAttribute _attribute;
        [SerializeField] private CoreAttribute _maxAttribute;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue)
        {
            if (_attribute != newAttributeValue.Attribute) return;
            var cacheDict = attributeSystem.GetAttributeIndexCache();
            ClampAttributeToMax(ref newAttributeValue, cacheDict, attributeSystem);
        }

        private void ClampAttributeToMax(ref AttributeValue newAttributeValue,
            Dictionary<CoreAttribute, int> cacheDict, AttributeSystemBehaviour attributeSystemBehaviour)
        {
            if (!cacheDict.TryGetValue(_attribute, out int attributeIdx)
                || !cacheDict.TryGetValue(_maxAttribute, out int maxAttributeIdx))
            {
                Debug.LogWarning($"Try to clamp attribute {_attribute.name} to max attribute {_maxAttribute.name} " +
                                 $"but one or both of them are not in the attribute system.");
                return;
            }

            var maxAttributeValue = attributeSystemBehaviour.AttributeValues[maxAttributeIdx];
            var preChange = newAttributeValue.Clone();

            if (newAttributeValue.CurrentValue >= maxAttributeValue.CurrentValue)
            {
                newAttributeValue.CurrentValue = maxAttributeValue.CurrentValue;
                Debug.Log(
                    $"Clamped current attribute {_attribute.name} from [{preChange.CurrentValue}] to [{newAttributeValue.CurrentValue}]");
            }

            if (newAttributeValue.BaseValue < maxAttributeValue.CurrentValue) return;

            newAttributeValue.BaseValue = maxAttributeValue.CurrentValue;
            Debug.Log(
                $"Clamped base {_maxAttribute.name} from [{preChange.BaseValue}] to [{newAttributeValue.BaseValue}].");
        }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) { }
    }
}