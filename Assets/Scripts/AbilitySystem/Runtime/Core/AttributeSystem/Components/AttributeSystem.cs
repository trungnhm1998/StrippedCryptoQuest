using System;
using System.Collections.Generic;
using UnityEngine;

namespace Indigames.AbilitySystem
{
    public class AttributeSystem : MonoBehaviour
    {
        [SerializeField] private List<AbstractAttributesEventChannel> _attributeEventChannels = new();
        public List<AbstractAttributesEventChannel> AttributeEventChannels { get => _attributeEventChannels; }

        /// <summary>
        /// If gameplay needs more attributes, add them here
        /// </summary>
        [SerializeField] private List<AttributeScriptableObject> _attributes = new();

        /// <summary>
        /// Value of the attributes above
        /// </summary>
        [SerializeField] private List<AttributeValue> _attributeValues = new();
        public List<AttributeValue> AttributeValues => _attributeValues;

        // Only cache the index of the attribute, not the attribute itself
        private Dictionary<AttributeScriptableObject, int> _attributeIndexCache = new();
        private bool _isCacheDirty = true;

        private void Awake()
        {
            InitializeAttributeValues();
            MarkCacheDirty();
            GetAttributeIndexCache();
        }

        private void InitializeAttributeValues()
        {
            _attributeValues = new List<AttributeValue>();
            foreach (var attribute in _attributes)
            {
                _attributeValues.Add(new AttributeValue(attribute));
            }
        }

        public void MarkCacheDirty()
        {
            _isCacheDirty = true;
        }

        private Dictionary<AttributeScriptableObject, int> GetAttributeIndexCache()
        {
            if (_isCacheDirty)
            {
                _attributeIndexCache.Clear();
                for (int i = 0; i < _attributeValues.Count; i++)
                {
                    _attributeIndexCache.Add(_attributeValues[i].Attribute, i);
                }

                _isCacheDirty = false;
            }

            return _attributeIndexCache;
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
            out AttributeValue outValue, EEffectStackingType stackingType = EEffectStackingType.External)
        {
            var cache = GetAttributeIndexCache();

            if (cache.TryGetValue(attributeToModify, out var index))
            {
                outValue = _attributeValues[index];
                switch (stackingType)
                {
                    case EEffectStackingType.External:
                        outValue.Modifier += modifier;
                        break;
                    case EEffectStackingType.Core:
                        outValue.CoreModifier += modifier;
                        break;
                }
                _attributeValues[index] = outValue;
                
                return true;
            }

            outValue = new AttributeValue();
            return false;
        }

        public bool HasAttribute(AttributeScriptableObject attributeSO)
        {
            var cache = GetAttributeIndexCache();
            return cache.TryGetValue(attributeSO, out _);
        }

        /// <summary>
        /// Add attributes to this attribute system. Duplicates are ignored.
        /// </summary>
        /// <param name="attributes">Attributes to add</param>
        public void AddAttributes(params AttributeScriptableObject[] attributes)
        {
            // If this attribute already exists, we don't need to add it. For that, we need to make sure the cache is up to date.
            var cache = GetAttributeIndexCache();

            foreach (var attributeToAdd in attributes)
            {
                if (!cache.ContainsKey(attributeToAdd))
                {
                    MarkCacheDirty();
                    _attributes.Add(attributeToAdd);
                    _attributeValues.Add(new AttributeValue(attributeToAdd));
                }
            }
        }

        private List<AttributeValue> _previousValues = new List<AttributeValue>();
        private const float TOLERATED_DIFFERENCE = 0.01f;

        /// <summary>
        /// Force update single attribute
        /// </summary>
        public void UpdateAttributeCurrentValue(AttributeScriptableObject attribute)
        {
            if (!GetAttributeIndexCache().TryGetValue(attribute, out var index))
                return;
            AttributeValue attributeValue = _attributeValues[index];
            var previousValue = attributeValue.Clone();
            foreach (var eventChannel in _attributeEventChannels)
            {
                eventChannel.PreAttributeChanged(this, _previousValues, ref _attributeValues);
            }

            _attributeValues[index] = AttributeSystemHelper.CalculateCurrentAttributeValue(attributeValue);

            if (!(Math.Abs(attributeValue.CurrentValue - previousValue.CurrentValue) > TOLERATED_DIFFERENCE))
                return;

            attribute.RaiseValueChangedEvent(this, previousValue, attributeValue);
        }

        public void UpdateAllAttributeCurrentValues()
        {
            _previousValues.Clear();
            for (int i = 0; i < _attributeValues.Count; i++)
            {
                var _attributeValue = _attributeValues[i];
                _previousValues.Add(_attributeValue.Clone());
            }

            foreach (var eventChannel in _attributeEventChannels)
            {
                eventChannel.PreAttributeChanged(this, _previousValues, ref _attributeValues);
            }

            for (int i = 0; i < _attributeValues.Count; i++)
            {
                var _attributeValue = _attributeValues[i];
                AttributeScriptableObject attributeScriptableObject = _attributeValue.Attribute;
                AttributeValue calculateCurrentAttributeValue = AttributeSystemHelper.CalculateCurrentAttributeValue(_attributeValue);
                _attributeValues[i] = calculateCurrentAttributeValue;

                if (Math.Abs(_attributeValues[i].CurrentValue - _previousValues[i].CurrentValue) > TOLERATED_DIFFERENCE)
                {
                    Debug.Log(
                        $"Attribute {_attributeValues[i].Attribute.name} changed from {_previousValues[i].CurrentValue} to {_attributeValues[i].CurrentValue}");
                    attributeScriptableObject
                        .RaiseValueChangedEvent(this, _previousValues[i], _attributeValues[i]);
                }
            }
        }

        private AttributeScriptableObject _cachedAttribute;
        private int _cachedIndex = -1;

        public bool GetAttributeValue(AttributeScriptableObject attribute, out AttributeValue value)
        {
            if (_cachedAttribute == attribute && _cachedIndex != -1)
            {
                value = _attributeValues[_cachedIndex];
                return true;
            }

            var cache = GetAttributeIndexCache();
            if (cache.TryGetValue(attribute, out var index))
            {
                _cachedIndex = index;
                _cachedAttribute = attribute;
                value = _attributeValues[index].Clone();
                return true;
            }

            value = new AttributeValue();
            return false;
        }

        public void SetAttributeBaseValue(AttributeScriptableObject attribute, float value)
        {
            var cache = GetAttributeIndexCache();
            if (!cache.TryGetValue(attribute, out var index)) return;
            var attributeValue = _attributeValues[index];
            attributeValue.BaseValue = value;
            _attributeValues[index] = attributeValue;
        }

        public void ResetAllAttributes()
        {
            for (int i = 0; i < _attributeValues.Count; i++)
            {
                var defaultAttributeValue = new AttributeValue(_attributeValues[i].Attribute);
                _attributeValues[i] = defaultAttributeValue;
            }
        }

        /// <summary>
        /// loop through _attributeValues and reset all of it modifiers
        /// </summary>
        public void ResetAttributeModifiers()
        {
            for (int i = 0; i < _attributeValues.Count; i++)
            {
                var attributeValue = _attributeValues[i];
                attributeValue.Modifier = attributeValue.CoreModifier = new Modifier();
                _attributeValues[i] = attributeValue;
            }
        }

        public List<AttributeScriptableObject> GetAttributes()
        {
            return _attributes;
        }

        /// <summary>
        /// Use lateUpdate to make sure skills/effects finishes their calculations before we update the attribute values
        /// </summary>
        private void LateUpdate()
        {
            UpdateAllAttributeCurrentValues();
        }
    }
}