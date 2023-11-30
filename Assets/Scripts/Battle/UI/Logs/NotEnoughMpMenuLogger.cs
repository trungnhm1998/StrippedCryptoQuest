using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class NotEnoughMpMenuLog : ActionBase { }

    public class NotEnoughMpMenuLogger : MonoBehaviour
    {
        [SerializeField] private UnityEvent<LocalizedString> _presentLoggerEvent;
        [SerializeField] private LocalizedString _localizedLog;
        private TinyMessageSubscriptionToken _token;

        private void OnEnable()
        {
            _token = ActionDispatcher.Bind<NotEnoughMpMenuLog>(HandleLog);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_token);
        }

        private void HandleLog(NotEnoughMpMenuLog _)
        {
            var msg = new LocalizedString(_localizedLog.TableReference, _localizedLog.TableEntryReference);
            _presentLoggerEvent.Invoke(msg);
        }
    }
}