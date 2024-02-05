using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Character
{
    public class AttributeChangeEvent : MonoBehaviour
    {
        [Serializable]
        public struct AttributeEvent
        {
            public AttributeScriptableObject Attribute;
            public UnityEvent<float> Event;
        }

        [SerializeField] private AttributeSystemBehaviour _attributeSystemReference;
        [SerializeField] private AttributeEvent[] _onAttributesChanged;
        private AttributeSystemBehaviour.PostAttributeChangeDelegate _handler;

        private Dictionary<AttributeScriptableObject, UnityEvent<float>> _attributeEvents = new();

        public Dictionary<AttributeScriptableObject, UnityEvent<float>> AttributeEvents
        {
            get
            {
                if (_attributeEvents.Count != 0) return _attributeEvents;
                foreach (var attributeEvent in _onAttributesChanged)
                {
                    if (_attributeEvents.ContainsKey(attributeEvent.Attribute))
                    {
                        Debug.LogWarning($"Duplicate attribute event for {attributeEvent.Attribute.name} in {name}");
                        continue;
                    }

                    _attributeEvents.Add(attributeEvent.Attribute, attributeEvent.Event);
                }

                return _attributeEvents;
            }
        }

        public AttributeSystemBehaviour AttributeSystemReference
        {
            get => _attributeSystemReference;
            set
            {
                ClearChangeHandler();

                _attributeSystemReference = value;

                RegisterChangeHandler();
                UpdateAttributesValue();
            }
        }

        private void Awake() { }

        private void OnEnable() => RegisterChangeHandler();

        private void OnDisable() => ClearChangeHandler();

        private void OnDestroy() => ClearChangeHandler();

        private void RegisterChangeHandler()
        {
            if (_handler == null)
                _handler = OnAttributeChanged;

            if (_attributeSystemReference)
                _attributeSystemReference.PostAttributeChange += _handler;
        }

        private void ClearChangeHandler()
        {
            if (_attributeSystemReference)
                _attributeSystemReference.PostAttributeChange -= _handler;
        }
        
        /// <summary>
        /// Set all observing attribute value to its current value
        /// so PostAttributeChange will be raised once when setup UI to update attribute display
        /// </summary>
        public void UpdateAttributesValue()
        {
            var attributeSystem = _attributeSystemReference;
            foreach (var attributeEvent in AttributeEvents)
            {
                var attribute = attributeEvent.Key;
                if (!attributeSystem.TryGetAttributeValue(attribute, out var attributeValue)) continue;
                attributeSystem.SetAttributeValue(attribute, attributeValue);
            }
        }

        private void OnAttributeChanged(
            AttributeScriptableObject attribute,
            AttributeValue oldValue,
            AttributeValue newValue)
        {
            if (AttributeEvents.TryGetValue(attribute, out var unityEvent))
            {
                unityEvent.Invoke(newValue.CurrentValue);
            }
        }
    }
}