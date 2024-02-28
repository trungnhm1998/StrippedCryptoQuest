﻿using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Battle.Events
{
    public class AttributeChangeEvent : AttributesEventBase
    {
        public delegate void ChangeEvent(AttributeSystemBehaviour owner, AttributeValue oldValue,
            AttributeValue newValue);

        public event ChangeEvent Changed;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue) { }

        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (Mathf.Approximately(oldAttributeValue.CurrentValue, newAttributeValue.CurrentValue) == false)
            {
                Debug.Log(
                    $"Attribute {attributeSystem.name}.{newAttributeValue.Attribute.name} changed from {oldAttributeValue.CurrentValue} to {newAttributeValue.CurrentValue}");
                attributeSystem.TryGetAttributeValue(newAttributeValue.Attribute, out var attribute);
                Debug.Log($"Core modifier: [{attribute.CoreModifier.ToString()}]");
                Debug.Log($"External modifier: [{attribute.ExternalModifier.ToString()}]");
            }
#endif
            Changed?.Invoke(attributeSystem, oldAttributeValue, newAttributeValue);
        }
    }
}