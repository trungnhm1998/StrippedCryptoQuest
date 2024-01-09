using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class CanNotEscapeLocationLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;
        [SerializeField] private LocalizedString _localizedLog;
        private TinyMessageSubscriptionToken _escapeAbilityEvent;

        private void OnEnable()
        {
            _escapeAbilityEvent = BattleEventBus.SubscribeEvent<CanNotEscapeEvent>(HandleLogger);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_escapeAbilityEvent);
        }

        private void HandleLogger(CanNotEscapeEvent ctx)
        {
            var msg = new LocalizedString(_localizedLog.TableReference, _localizedLog.TableEntryReference);
            _presentLoggerEvent.Invoke(msg);
        }
    }
}