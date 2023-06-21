using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    [CreateAssetMenu(menuName = "Indigames Ability System/Attribute")]
    public class AttributeScriptableObject : ScriptableObject
    {
        public struct AttributeEventArgs
        {
            public AttributeSystem System;
            public AttributeValue NewValue;
            public AttributeValue OldValue;
        }
        public Action<AttributeEventArgs> ValueChangeEvent;

        public void RaiseValueChangedEvent(AttributeSystem system, AttributeValue oldValue, AttributeValue newValue)
        {
            ValueChangeEvent?.Invoke(new AttributeEventArgs
            {
                System = system,
                OldValue = oldValue,
                NewValue = newValue
            });
        }

        public virtual AttributeValue CalculateInitialValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValues)
        {
            return attributeValue;
        }

        public virtual AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValues)
        {
            if (attributeValue.Modifier.Overriding != 0)
            {
                attributeValue.CurrentValue = attributeValue.Modifier.Overriding;
                return attributeValue;
            }
            if (attributeValue.CoreModifier.Overriding != 0)
            {
                attributeValue.CurrentValue = attributeValue.CoreModifier.Overriding;
                return attributeValue;
            }

            var coreValue = (attributeValue.BaseValue + attributeValue.CoreModifier.Additive) *
                                          (attributeValue.CoreModifier.Multiplicative + 1);


            attributeValue.CurrentValue = (coreValue + attributeValue.Modifier.Additive) *
                                          (attributeValue.Modifier.Multiplicative + 1);
            return attributeValue;
        }
    }
}