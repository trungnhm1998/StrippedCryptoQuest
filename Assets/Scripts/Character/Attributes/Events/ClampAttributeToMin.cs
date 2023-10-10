using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    [CreateAssetMenu(menuName = "Crypto Quest/Character/Attribute Events/Clamp Attribute To Min")]
    public class ClampAttributeToMin : AttributesEventBase
    {
        [SerializeField] private CoreAttribute _attribute;
        [SerializeField] private float _minValue;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue)
        {
            if (_attribute != newAttributeValue.Attribute) return;
            var cacheDict = attributeSystem.GetAttributeIndexCache();
            ClampAttributeValueToMin(ref newAttributeValue, cacheDict, attributeSystem);
        }

        private void ClampAttributeValueToMin(ref AttributeValue newAttributeValue,
            Dictionary<CoreAttribute, int> cacheDict, AttributeSystemBehaviour attributeSystem)
        {
            if (!cacheDict.TryGetValue(_attribute, out int attributeIdx))
            {
                Debug.LogWarning($"Try to clamp attribute {_attribute.name} to min value [{_minValue}] " +
                                 $"but the attribute is not in the attribute system.");
                return;
            }

            var preChange = newAttributeValue.Clone();

            if (newAttributeValue.CurrentValue <= _minValue)
            {
                newAttributeValue.CurrentValue = _minValue;
                Debug.Log(
                    $"Clamped current {_attribute.name} from [{preChange.BaseValue}] to [{_minValue}].");
            }

            if (newAttributeValue.BaseValue > _minValue) return;

            newAttributeValue.BaseValue = _minValue;
            Debug.Log(
                $"Clamped base {_attribute.name} from [{preChange.BaseValue}] to [{_minValue}].");
        }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) { }
    }
}