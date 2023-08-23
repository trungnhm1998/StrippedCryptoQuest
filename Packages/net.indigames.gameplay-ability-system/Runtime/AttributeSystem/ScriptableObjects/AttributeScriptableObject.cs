using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects
{
    /// <summary>
    /// Typesafe event handler
    /// </summary>
    public class AttributeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// The owner
        /// </summary>
        public AttributeSystemBehaviour AttributeSystem { get; set; }

        public float Value { get; set; }
    }

    [CreateAssetMenu(menuName = "Indigames Ability System/Attributes/Attribute")]
    public class AttributeScriptableObject : ScriptableObject
    {
        public struct AttributeEventArgs
        {
            public AttributeSystemBehaviour System;
            public AttributeValue NewValue;
            public AttributeValue OldValue;
        }

        public event EventHandler AttributeChanged;
        public event Action<AttributeEventArgs> ValueChangeEvent;

        public void OnValueChanged(AttributeSystemBehaviour system, AttributeValue oldValue, AttributeValue newValue)
        {
            ValueChangeEvent?.Invoke(new AttributeEventArgs
            {
                System = system,
                OldValue = oldValue,
                NewValue = newValue
            });
        }

        /// <summary>
        /// This is called before the attribute value is changed.
        /// This is a good place to add your own logic to modify the value.
        /// for example, you could clamp the value to a certain range or based on max attribute.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnAttributeChanged(object sender, AttributeChangedEventArgs e)
        {
            EventHandler handler = AttributeChanged;
            if (handler != null)
            {
                handler(sender, e);
            }
        }

        /// <summary>
        /// Called by <see cref="AttributeSystemBehaviour.InitializeAttributeValues"/> to calculate the initial value of the attribute.
        /// There would be a hidden bug here when the current attribute value depends on other attributes.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <param name="otherAttributeValues"></param>
        /// <returns></returns>
        public virtual AttributeValue CalculateInitialValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValues)
        {
            return attributeValue;
        }

        /// <summary>
        /// Called by <see cref="AttributeSystemBehaviour.UpdateAttributeValues"/> to calculate the current value of the attribute.
        /// return a new <see cref="AttributeValue"/> with the current value set.
        /// Wrap the base value with core modifier first <a herf="https://www.youtube.com/watch?v=JgSvuSaXs3E">source</a> before applying external modifier.
        /// </summary>
        /// <param name="attributeValue"></param>
        /// <param name="otherAttributeValuesInSystem"></param>
        /// <returns></returns>
        public virtual AttributeValue CalculateCurrentAttributeValue(AttributeValue attributeValue,
            List<AttributeValue> otherAttributeValuesInSystem)
        {
            if (attributeValue.ExternalModifier.Overriding != 0)
            {
                attributeValue.CurrentValue = attributeValue.ExternalModifier.Overriding;
                return attributeValue;
            }

            // order matters here, we want to override external with core
            if (attributeValue.CoreModifier.Overriding != 0)
            {
                attributeValue.CurrentValue = attributeValue.CoreModifier.Overriding;
                return attributeValue;
            }

            var coreValue = (attributeValue.BaseValue + attributeValue.CoreModifier.Additive) *
                            (attributeValue.CoreModifier.Multiplicative + 1);

            attributeValue.CurrentValue = (coreValue + attributeValue.ExternalModifier.Additive) *
                                          (attributeValue.ExternalModifier.Multiplicative + 1);
            return attributeValue;
        }
    }
}