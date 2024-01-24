using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class CastSkillFailedLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _failedMessage;
        private TinyMessageSubscriptionToken _failedEvent;

        private void Awake()
        {
            _failedEvent = BattleEventBus.SubscribeEvent<CastSkillFailedEvent>(LogCastFailed);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_failedEvent);
        }
        
        private void LogCastFailed(CastSkillFailedEvent ctx)
        {
            Logger.QueueLog(_failedMessage);
        }
    }
}