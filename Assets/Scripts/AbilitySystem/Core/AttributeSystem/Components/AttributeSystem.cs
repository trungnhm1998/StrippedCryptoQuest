using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public class AttributeSystem : MonoBehaviour
    {
        [SerializeField] private List<AbstractAttributesEventChannel> _eventChannels;

        /// <summary>
        /// If gameplay needs more attributes, add them here
        /// </summary>
        [SerializeField] private List<AttributeScriptableObject> attributes;

        /// <summary>
        /// value of the attributes above
        /// </summary>
        [SerializeField] private List<AttributeValue> attributeValues;

        public List<AttributeValue> AttributeValues => attributeValues;

        // Only cache the index of the attribute, not the attribute itself
        public Dictionary<AttributeScriptableObject, int> attributeIndexCache { get; private set; } =
            new Dictionary<AttributeScriptableObject, int>();
        public List<AbstractAttributesEventChannel> EventChannels { get => _eventChannels; }

        private bool _isCacheDirty = true;

        public void MarkCacheDirty()
        {
            _isCacheDirty = true;
        }

        /// <summary>
        /// Try to find the attribute in the system and update it Modifier value 
        /// </summary>
        /// <param name="attributeToModify"></param>
        /// <param name="modifier"></param>
        /// <param name="outValue"></param>
        /// <param name="stackingType"></param>
        /// <returns>True if the attribute what to modify is in the system</returns>
        public bool AddModifierToAttribute(Modifier modifier, AttributeScriptableObject attributeToModify,
            out AttributeValue outValue, EffectStackingType stackingType = EffectStackingType.External)
        {
            var cache = GetAttributeIndexCache();

            if (cache.TryGetValue(attributeToModify, out var index))
            {
                outValue = attributeValues[index];
                switch (stackingType)
                {
                    case EffectStackingType.External:
                        outValue.Modifier += modifier;
                        break;
                    case EffectStackingType.Core:
                        outValue.CoreModifier += modifier;
                        break;
                }

                attributeValues[index] = outValue;
                return true;
            }

            outValue = new AttributeValue();
            return false;
        }

        public bool HasAttribute(AttributeScriptableObject attributeSO)
        {
            return attributeIndexCache.TryGetValue(attributeSO, out _);
        }

        /// <summary>
        /// Add attributes to this attribute system.  Duplicates are ignored.
        /// </summary>
        /// <param name="_attributes">Attributes to add</param>
        public void AddAttributes(params AttributeScriptableObject[] _attributes)
        {
            // If this attribute already exists, we don't need to add it.  For that, we need to make sure the cache is up to date.
            var cache = GetAttributeIndexCache();

            foreach (var attributeToAdd in _attributes)
            {
                if (!cache.ContainsKey(attributeToAdd))
                {
                    MarkCacheDirty();
                    attributes.Add(attributeToAdd);
                    attributeValues.Add(new AttributeValue(attributeToAdd));
                }
            }
        }

        /// <summary>
        /// Force update single attribute
        /// </summary>
        public void UpdateAttributeCurrentValue(AttributeScriptableObject attribute)
        {
            if (!GetAttributeIndexCache().TryGetValue(attribute, out var index))
                return;
            AttributeValue attributeValue = attributeValues[index];
            var previousValue = attributeValue.Clone();
            attributeValues[index] = attributeValue.Attribute
                .CalculateCurrentAttributeValue(attributeValue, attributeValues);

            if (!(Math.Abs(attributeValue.CurrentValue - previousValue.CurrentValue) > TOLERATED_DIFFERENCE))
                return;

            attribute.RaiseValueChangedEvent(this, previousValue, attributeValue);
            foreach (var eventChannel in _eventChannels)
            {
                eventChannel.PreAttributeChanged(this, _previousValues, ref attributeValues);
            }
        }

        private List<AttributeValue> _previousValues = new List<AttributeValue>();
        private const float TOLERATED_DIFFERENCE = 0.01f;

        public void UpdateAttributeCurrentValues()
        {
            _previousValues.Clear();
            for (int i = 0; i < attributeValues.Count; i++)
            {
                var _attributeValue = attributeValues[i];
                _previousValues.Add(_attributeValue.Clone());
                AttributeScriptableObject attributeScriptableObject = _attributeValue.Attribute;
                AttributeValue calculateCurrentAttributeValue = attributeScriptableObject.CalculateCurrentAttributeValue(_attributeValue, attributeValues);
                attributeValues[i] = calculateCurrentAttributeValue;
            }

            foreach (var eventChannel in _eventChannels)
            {
                eventChannel.PreAttributeChanged(this, _previousValues, ref attributeValues);
            }

            for (int i = 0; i < attributeValues.Count; i++)
            {
                var _attributeValue = attributeValues[i];
                AttributeScriptableObject attributeScriptableObject = _attributeValue.Attribute;
                if (Math.Abs(attributeValues[i].CurrentValue - _previousValues[i].CurrentValue) > TOLERATED_DIFFERENCE)
                {
                    Debug.Log(
                        $"Attribute {attributeValues[i].Attribute.name} changed from {_previousValues[i].CurrentValue} to {attributeValues[i].CurrentValue}");
                    attributeScriptableObject
                        .RaiseValueChangedEvent(this, _previousValues[i], attributeValues[i]);
                }
            }

        }

        private AttributeScriptableObject _cachedAttribute;
        private int _cachedIndex = -1;

        public bool GetAttributeValue(AttributeScriptableObject attribute, out AttributeValue value)
        {
            if (_cachedAttribute == attribute && _cachedIndex != -1)
            {
                value = attributeValues[_cachedIndex];
                return true;
            }

            var cache = GetAttributeIndexCache();
            if (cache.TryGetValue(attribute, out var index))
            {
                _cachedIndex = index;
                _cachedAttribute = attribute;
                value = attributeValues[index].Clone();
                return true;
            }

            value = new AttributeValue();
            return false;
        }

        public void SetAttributeBaseValue(AttributeScriptableObject attribute, float value)
        {
            var cache = GetAttributeIndexCache();
            if (!cache.TryGetValue(attribute, out var index)) return;

            var attributeValue = attributeValues[index];
            // Debug.Log(
            // $"AttributeSystem::SetAttributeBaseValue::{name} {attribute.name} current {attributeValue.CurrentValue} to {value}");
            attributeValue.BaseValue = value;
            attributeValues[index] = attributeValue;
        }

        private void Awake()
        {
            InitializeAttributeValues();
            MarkCacheDirty();
            GetAttributeIndexCache();
        }

        private void InitializeAttributeValues()
        {
            attributeValues = new List<AttributeValue>();
            foreach (var attribute in attributes)
            {
                attributeValues.Add(new AttributeValue(attribute));
            }
        }

        private Dictionary<AttributeScriptableObject, int> GetAttributeIndexCache()
        {
            if (_isCacheDirty)
            {
                attributeIndexCache.Clear();
                for (int i = 0; i < attributeValues.Count; i++)
                {
                    attributeIndexCache.Add(attributeValues[i].Attribute, i);
                }

                _isCacheDirty = false;
            }

            return attributeIndexCache;
        }

        public void ResetAllAttributes()
        {
            // loop through all attributeValue and reset them
            for (int i = 0; i < attributeValues.Count; i++)
            {
                var defaultAttributeValue = new AttributeValue(attributeValues[i].Attribute);
                attributeValues[i] = defaultAttributeValue;
            }
        }

        /// <summary>
        /// loop through attributeValues and reset all of it modifiers
        /// </summary>
        public void ResetAttributeModifiers()
        {
            for (int i = 0; i < attributeValues.Count; i++)
            {
                var attributeValue = attributeValues[i];
                attributeValue.Modifier = attributeValue.CoreModifier = new Modifier();
                attributeValues[i] = attributeValue;
            }
        }

        public List<AttributeScriptableObject> GetAttributes()
        {
            return attributes;
        }

        /// <summary>
        /// Use lateUpdate to make sure skills/effects finishes their calculations before we update the attribute values
        /// </summary>
        private void LateUpdate()
        {
            UpdateAttributeCurrentValues();
        }
    }
}