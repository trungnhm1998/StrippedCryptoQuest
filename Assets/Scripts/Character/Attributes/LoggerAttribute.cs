using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using CoreAttribute = IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects.AttributeScriptableObject;

namespace CryptoQuest.Character.Attributes
{
    public class LoggerAttribute : AttributesEventBase
    {
        public Action<AttributeSystemBehaviour, AttributeValue> OnAttributeChanged;

        public override void PreAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue newAttributeValue)
        {
            OnAttributeChanged?.Invoke(attributeSystem, newAttributeValue);
        }


        public override void PostAttributeChange(AttributeSystemBehaviour attributeSystem,
            ref AttributeValue oldAttributeValue,
            ref AttributeValue newAttributeValue) { }
    }
}