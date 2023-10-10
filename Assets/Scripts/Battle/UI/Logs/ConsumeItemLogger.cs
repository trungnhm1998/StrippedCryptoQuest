using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class ConsumeItemLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _consumeItemMessage;
        private TinyMessageSubscriptionToken _useConsumeItemEvent;

        private void OnEnable()
        {
            _useConsumeItemEvent = BattleEventBus.SubscribeEvent<ConsumeItemEvent>(LogConsumeItem);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_useConsumeItemEvent);
        }

        private void LogConsumeItem(ConsumeItemEvent consumeEvent)
        {
            var castMessage = _consumeItemMessage;
            castMessage.Add(Constants.CHARACTER_NAME, consumeEvent.Character.LocalizedName);
            castMessage.Add(Constants.ITEM_NAME, consumeEvent.ItemInfo.Data.DisplayName);
            castMessage.Add(Constants.CHARACTER_TARGET_NAME, consumeEvent.Target.LocalizedName);
            Logger.AppendLog(castMessage);
        }
    }
}