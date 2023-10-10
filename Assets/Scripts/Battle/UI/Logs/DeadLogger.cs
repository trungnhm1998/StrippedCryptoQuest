using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class DeadLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _deadLogMessage;
        private TinyMessageSubscriptionToken _deadEvent;

        private void OnEnable()
        {
            _deadEvent = BattleEventBus.SubscribeEvent<DeadEvent>(LogDead);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_deadEvent);
        }

        private void LogDead(DeadEvent ctx)
        {
            var castMessage = _deadLogMessage;
            castMessage.Add(Constants.CHARACTER_NAME, ctx.Character.LocalizedName);
            Logger.AppendLog(castMessage);
        }
    }
}
