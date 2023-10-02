using System.Collections.Generic;
using CryptoQuest.Character.Attributes.ClampEventAction;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects;
using UnityEngine;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    public class ClampAttribute : AttributesEventBase
    {
        [SerializeField] private CoreAttribute _attribute;
        [SerializeField] private CoreAttribute _maxAttribute;
        
        [field: SerializeReference, ReferenceEnum]
        public IClampAction ClampAction { get; set; } = new DoNothing();

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
                ClampAction?.OnClampFailed(attributeSystemBehaviour);
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

            if (newAttributeValue.BaseValue >= maxAttributeValue.BaseValue)
            {
                newAttributeValue.BaseValue = maxAttributeValue.BaseValue;
                Debug.Log(
                    $"Clamped base {_maxAttribute.name} from [{preChange.BaseValue}] to [{newAttributeValue.BaseValue}].");
                ClampAction?.OnClampSuccess(attributeSystemBehaviour);
                return;
            }

            ClampAction?.OnClampFailed(attributeSystemBehaviour);
        }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) { }
    }
}