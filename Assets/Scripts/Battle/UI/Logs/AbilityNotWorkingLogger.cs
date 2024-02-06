using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using TinyMessenger;
using IndiGames.Core.Events;
using CryptoQuest.AbilitySystem.Abilities.Conditions;

namespace CryptoQuest.Battle.UI.Logs
{
    public class AbilityNotWorkingLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;

        [SerializeField] private LocalizedString _localizedLog;

        private TinyMessageSubscriptionToken _abilityFailedEvent;

        private void OnEnable()
        {
            _abilityFailedEvent = ActionDispatcher.Bind<AbilityConditionFailed>(_
                => _presentLoggerEvent.Invoke(_localizedLog));
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_abilityFailedEvent);
        }
    }
}