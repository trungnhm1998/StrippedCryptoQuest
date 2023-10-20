using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class GuardedLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _logMessage;
        private TinyMessageSubscriptionToken _token;

        private void OnEnable()
        {
            _token = BattleEventBus.SubscribeEvent<GuardedEvent>(Listener);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_token);
        }

        private void Listener(GuardedEvent ctx)
        {
            var guardMessage = new LocalizedString(_logMessage.TableReference, _logMessage.TableEntryReference)
                { { Constants.CHARACTER_NAME, ctx.Character.LocalizedName } };
            Logger.QueueLog(guardMessage);
        }
    }
}