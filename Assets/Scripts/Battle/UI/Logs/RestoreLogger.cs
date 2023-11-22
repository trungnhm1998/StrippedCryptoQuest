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
    public class RestoreLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;
        [SerializeField] private LocalizedString _localizedLog;
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
            if (!_attributeConfigMapping.TryGetMap(changedAttribute, out var map)) return;

            var isRestored = oldValue.CurrentValue < newValue.CurrentValue;
            var isRevived = oldValue.CurrentValue == 0 && changedAttribute == AttributeSets.Health;
            if (!isRestored || isRevived) return;

            if (!attributeSystem.TryGetComponent<Components.Character>(out var character)) return;

            var characterName = character.LocalizedName;
            var msg = new LocalizedString(_localizedLog.TableReference, _localizedLog.TableEntryReference)
            {
                { Constants.CHARACTER_NAME, characterName },
                { Constants.ATTRIBUTE_NAME, map.Name }
            };

            _presentLoggerEvent.Invoke(msg);
        }
    }
}