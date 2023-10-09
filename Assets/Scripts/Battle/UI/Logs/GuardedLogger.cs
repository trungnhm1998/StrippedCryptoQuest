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
        private string _characterName;
        public new string name => _characterName;

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
            _characterName = ctx.Character.DisplayName;
            Logger.AppendLog(_logMessage);
        }
    }
}