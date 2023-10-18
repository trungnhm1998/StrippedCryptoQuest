using System.Collections.Generic;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Character;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.UI.Logs;

namespace CryptoQuest.Battle.UI.Logs
{
    public class StatChangedLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private LocalizedString _increasedLog;
        [SerializeField] private LocalizedString _decreasedLog
        ;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;
        [SerializeField] private AttributeScriptableObject[] _affectedAttributes;

        private Dictionary<int, LocalizedString> _attributesNameDict = new();

        private void Awake()
        {
            _attributesNameDict = _affectedAttributes.ToDictionary(keySelector:attr => attr.GetInstanceID(),
                attr => attr.DisplayName);
        }

        private void OnEnable()
        {
            _attributeChangeEvent.Changed += OnAttributeChanged;
        }

        private void OnDisable()
        {
            _attributeChangeEvent.Changed -= OnAttributeChanged;
        }

        private void OnAttributeChanged(AttributeSystemBehaviour attributeSystem, AttributeValue oldValue,
            AttributeValue newValue)
        {
            var changedAttribute = oldValue.Attribute;
            if (!_attributesNameDict.TryGetValue(changedAttribute.GetInstanceID(), out var attributeName))
                return;
            
            if (Mathf.Approximately(oldValue.CurrentValue, newValue.CurrentValue)) return;
            var isJustInit = oldValue.CurrentValue == 0;
            if (isJustInit) return;
            
            var isIncrease = oldValue.CurrentValue < newValue.CurrentValue;
            var log = isIncrease ? _increasedLog : _decreasedLog;
            if (!attributeSystem.TryGetComponent(out Components.Character character)) return;
            
            log.Add(Constants.CHARACTER_NAME, character.LocalizedName);
            log.Add(Constants.ATTRIBUTE_NAME, attributeName);

            _presentLoggerEvent.Invoke(log);            
        }
    }
}