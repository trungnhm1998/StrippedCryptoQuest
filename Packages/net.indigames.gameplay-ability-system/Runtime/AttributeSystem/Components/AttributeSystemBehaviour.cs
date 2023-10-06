using System;
using System.Collections.Generic;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.AttributeSystem.Components
{
    /// <summary>
    /// Manage all the attributes in the system
    /// Each <see cref="AttributeScriptableObject"/> with have their corresponding <see cref="AttributeValue"/>
    /// <see cref="AttributeValue"/> will be created at runtime
    /// </summary>
    public partial class AttributeSystemBehaviour : MonoBehaviour
    {
        public delegate void PreAttributeChangeDelegate(AttributeScriptableObject attribute, AttributeValue newValue);

        public delegate void PostAttributeChangeDelegate(AttributeScriptableObject attribute, AttributeValue oldValue,
            AttributeValue newValue);

        public event PreAttributeChangeDelegate PreAttributeChange;
        public event PostAttributeChangeDelegate PostAttributeChange;
        [SerializeField] private bool _initOnAwake = true;
        [SerializeField] private bool _updateOnLateUpdate = false;

        /// <summary>
        /// Use this to clamp or doing any logic pre and post attribute change
        /// </summary>
        [SerializeField] private List<AttributesEventBase> _attributeEvents = new();

        /// <summary>
        /// If gameplay needs more attributes, add them here
        /// </summary>
        [SerializeField] private List<AttributeScriptableObject> _attributes = new();

        public List<AttributeScriptableObject> Attributes => _attributes;

        /// <summary>
        /// Value of the attributes above
        /// </summary>
        [SerializeField] private List<AttributeValue> _attributeValues = new();

        public List<AttributeValue> AttributeValues => _attributeValues;

        /// <summary>
        /// Only cache the index of the attribute, not the attribute itself.
        /// So I could modified the attribute value without having to update the cache
        /// </summary>
        private readonly Dictionary<AttributeScriptableObject, int> _attributeIndexCache = new();

        private bool _isCacheStale = true;

        private void Awake()
        {
            if (_initOnAwake) Init();
        }

        public virtual void Init()
        {
            _attributeIndexCache.Clear();
            InitializeAttributeValues();
            MarkCacheDirty();
            GetAttributeIndexCache();
        }

        private void InitializeAttributeValues()
        {
            _attributeValues = new List<AttributeValue>();
            var attributes = new List<AttributeScriptableObject>(_attributes);
            _attributes = new();
            for (var index = 0; index < attributes.Count; index++)
            {
                var attribute = attributes[index];
                AddAttribute(attribute);
            }
        }

        private void MarkCacheDirty()
        {
            _isCacheStale = true;
        }

        /// <summary>
        /// Get Attribute indices from the cache if the cache were dirty little ***
        /// we will UnStale the cache and update it
        /// </summary>
        /// <returns></returns>
        public Dictionary<AttributeScriptableObject, int> GetAttributeIndexCache()
        {
            if (!_isCacheStale) return _attributeIndexCache;

            _attributeIndexCache.Clear();
            for (int i = 0; i < _attributeValues.Count; i++)
            {
                _attributeIndexCache.Add(_attributeValues[i].Attribute, i);
            }

            _isCacheStale = false;

            return _attributeIndexCache;
        }

        /// <summary>
        /// Try to find the attribute in the system and update it Modifier value 
        /// </summary>
        /// <param name="attributeToModify"></param>
        /// <param name="modifier"></param>
        /// <param name="modifierType"></param>
        /// <returns>True if the attribute what to modify is in the system</returns>
        public bool TryAddModifierToAttribute(Modifier modifier, AttributeScriptableObject attributeToModify,
            EModifierType modifierType = EModifierType.External)
        {
            var cache = GetAttributeIndexCache();

            if (!cache.TryGetValue(attributeToModify, out var index)) return false;

            var attributeValue = _attributeValues[index];
            switch (modifierType)
            {
                case EModifierType.External:
                    attributeValue.ExternalModifier += modifier;
                    break;
                case EModifierType.Core:
                    attributeValue.CoreModifier += modifier;
                    break;
            }

            var attributeWithNewModifer =
                attributeValue.Attribute.CalculateCurrentAttributeValue(attributeValue, _attributeValues);
            SetAttributeValue(attributeToModify, attributeWithNewModifer);

            return true;
        }

        /// <summary>
        /// Check if the system has the attribute and return a copy of the attribute value
        /// </summary>
        /// <param name="attributeSO">Which Attribute</param>
        /// <param name="value">The value of teh Attribute in system</param>
        /// <returns></returns>
        public bool HasAttribute(AttributeScriptableObject attributeSO, out AttributeValue value)
        {
            var cache = GetAttributeIndexCache();
            if (cache.TryGetValue(attributeSO, out var index))
            {
                value = _attributeValues[index];
                return true;
            }

            value = new AttributeValue();
            return false;
        }

        /// <summary>
        /// Add attributes to this attribute system. Duplicates are ignored.
        /// We also don't want to add a duplicate attribute into the system
        /// </summary>
        /// <param name="attribute">The data defined attribute</param>
        public void AddAttribute(AttributeScriptableObject attribute)
        {
            // Update the cache to make sure we don't add duplicate attribute
            var cache = GetAttributeIndexCache();
            if (cache.ContainsKey(attribute))
            {
                Debug.LogWarning(
                    $"AttributeSystemBehaviour::AddAttributes::Try to add duplicate attribute {attribute.name} to the system {name}" +
                    $"\nSafe to ignore");
                return;
            }

            MarkCacheDirty();
            _attributes.Add(attribute);
            var calculateInitialValue =
                attribute.CalculateInitialValue(new AttributeValue(attribute), _attributeValues);
            _attributeValues.Add(calculateInitialValue);
        }

        /// <summary>
        /// For every <see cref="AttributeScriptableObject"/> in the system, create a new <see cref="AttributeValue"/>
        /// to represent a updated value at run time.
        ///
        /// TODO: Refactor to not use in update loop
        /// </summary>
        public void UpdateAttributeValues()
        {
            for (int i = 0; i < _attributeValues.Count; i++)
            {
                AttributeValue oldAttributeValue = _attributeValues[i];
                var evaluatedAttribute = oldAttributeValue
                    .Attribute.CalculateCurrentAttributeValue(oldAttributeValue, _attributeValues);

                UpdateAttributeValueIfNotEquals(evaluatedAttribute, oldAttributeValue);
            }
        }

        private void UpdateAttributeValueIfNotEquals(AttributeValue newValue, AttributeValue oldValue)
        {
            if (newValue.CurrentValue.NearlyEqual(oldValue.CurrentValue)) return;

            SetAttributeValue(oldValue.Attribute, newValue);
        }

        /// <summary>
        /// Get a copy of Attribute Value from the system
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetAttributeValue(AttributeScriptableObject attribute, out AttributeValue value)
        {
            var cache = GetAttributeIndexCache();
            if (cache.TryGetValue(attribute, out var index))
            {
                value = _attributeValues[index].Clone();
                return true;
            }

            value = new AttributeValue();
            Debug.LogWarning($"AttributeSystemBehaviour" +
                             $"::TryGetAttributeValue::Attribute {attribute.name} not found in the system");
            return false;
        }

        /// <summary>
        /// Forcefully update base value of an <see cref="AttributeValue"/> in the system
        /// Regards of forcefully still a viable options, developer should should effect system to modify the value
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public void SetAttributeBaseValue(AttributeScriptableObject attribute, float value)
        {
            if (attribute == null)
            {
                Debug.LogWarning($"AttributeSystemBehaviour::SetAttributeBaseValue::Attribute is null");
                return;
            }
            var cache = GetAttributeIndexCache();
            if (!cache.TryGetValue(attribute, out var index))
            {
                AddAttribute(attribute);
                index = _attributeValues.Count - 1;
            }

            var attributeValue = _attributeValues[index];
            attributeValue.BaseValue = value;
            var evaluatedAttribute = attribute.CalculateCurrentAttributeValue(attributeValue, _attributeValues);
            UpdateAttributeValueIfNotEquals(evaluatedAttribute, _attributeValues[index]);

            UpdateAttributeValues(); // This attribute could cause other attribute to change
        }

        /// <summary>
        /// Forcefully update the attribute value in the system
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="attributeValue"></param>
        public void SetAttributeValue(AttributeScriptableObject attribute, AttributeValue attributeValue)
        {
            var cache = GetAttributeIndexCache();
            if (!cache.TryGetValue(attribute, out var index)) return;

            foreach (var preAttributeChangeChannel in _attributeEvents)
            {
                preAttributeChangeChannel.PreAttributeChange(this, ref attributeValue);
            }

            var oldValue = _attributeValues[index];
            PreAttributeChange?.Invoke(oldValue.Attribute, attributeValue);
            _attributeValues[index] = attributeValue;
            PostAttributeChange?.Invoke(attributeValue.Attribute, oldValue, attributeValue);
            
            foreach (var preAttributeChangeChannel in _attributeEvents)
            {
                preAttributeChangeChannel.PostAttributeChange(this, ref oldValue, ref attributeValue);
            }
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
        /// This will also cause the <see cref="AttributeValue.CurrentValue"/> to be equals to <see cref="AttributeValue.BaseValue"/>
        /// </summary>
        public void ResetAttributeModifiers()
        {
            for (int i = 0; i < _attributeValues.Count; i++)
            {
                var attributeValue = _attributeValues[i];
                attributeValue.ExternalModifier = attributeValue.CoreModifier = new Modifier();
                _attributeValues[i] = attributeValue;
            }
        }

        /// <summary>
        /// Use lateUpdate to make sure Abilities/effects finishes their calculations before we update the attribute values
        /// Support realtime update by default
        /// </summary>
        [Obsolete]
        protected virtual void LateUpdate()
        {
            if (_updateOnLateUpdate) UpdateAttributeValues();
        }
    }
}