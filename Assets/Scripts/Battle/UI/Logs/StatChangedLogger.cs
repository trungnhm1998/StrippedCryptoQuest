using System;
using System.Linq;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class StatChangedLogger : MonoBehaviour
    {
        public event Action<Components.Character, LocalizedString> StatsChangedWithSameValue;
        public event Action<Components.Character, LocalizedString> StatsChanged;

        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private LocalizedString _increasedLog;
        [SerializeField] private LocalizedString _decreasedLog;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;
        [SerializeField] private AttributeScriptableObject[] _affectedAttributes;
        [SerializeField] private AttributeConfigMapping _attributeConfigMapping;

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
            if (_affectedAttributes.Contains(changedAttribute) == false) return;
            if (!_attributeConfigMapping.TryGetMap(changedAttribute, out var map))
                return;

            if (!attributeSystem.TryGetComponent(out Components.Character character)) return;

            if (Mathf.Approximately(oldValue.CurrentValue, newValue.CurrentValue))
            {
                StatsChangedWithSameValue?.Invoke(character, map.Name);
                return;
            }

            var isIncrease = oldValue.CurrentValue < newValue.CurrentValue;
            var localizedLog = isIncrease ? _increasedLog : _decreasedLog;
            var log = new LocalizedString(localizedLog.TableReference, localizedLog.TableEntryReference)
            {
                { Constants.CHARACTER_NAME, character.LocalizedName },
                { Constants.ATTRIBUTE_NAME, map.Name }
            };

            _presentLoggerEvent.Invoke(log);
            StatsChanged?.Invoke(character, map.Name);
        }
    }
}