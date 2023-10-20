using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Battle.Character;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class ReviveLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private LocalizedString _localizedLog;
        [SerializeField] private AttributeChangeEvent _attributeChangeEvent;

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
            if (changedAttribute != AttributeSets.Health) return;

            var isRevived = oldValue.CurrentValue == 0 && newValue.CurrentValue > 0;
            if (!isRevived) return;
            if (!attributeSystem.TryGetComponent<HeroBehaviour>(out var hero)) return;

            var heroName = hero.DetailsInfo.LocalizedName;
            var msg = new LocalizedString(_localizedLog.TableReference, _localizedLog.TableEntryReference)
                { { Constants.CHARACTER_NAME, heroName } };

            _presentLoggerEvent.Invoke(msg);
        }
    }
}