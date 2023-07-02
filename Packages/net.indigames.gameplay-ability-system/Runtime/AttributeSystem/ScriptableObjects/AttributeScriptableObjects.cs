using System;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Indigames Ability System/Attributes/Attribute")]
    public class AttributeScriptableObject : ScriptableObject
    {
        public struct AttributeEventArgs
        {
            public AttributeSystemBehaviour System;
            public AttributeValue NewValue;
            public AttributeValue OldValue;
        }
        public Action<AttributeEventArgs> ValueChangeEvent;

        public void RaiseValueChangedEvent(AttributeSystemBehaviour system, AttributeValue oldValue, AttributeValue newValue)
        {
            ValueChangeEvent?.Invoke(new AttributeEventArgs
            {
                System = system,
                OldValue = oldValue,
                NewValue = newValue
            });
        }
    }
}