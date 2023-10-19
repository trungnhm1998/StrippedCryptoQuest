using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class ConsumeItemLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _consumeItemMessage;
        [SerializeField] private LocalizedString _consumeItemFailMessage;
        private TinyMessageSubscriptionToken _useConsumeItemEvent;
        private TinyMessageSubscriptionToken _consumeItemFailEvent;

        private void OnEnable()
        {
            _useConsumeItemEvent = BattleEventBus.SubscribeEvent<ConsumeItemEvent>(LogConsumeItem);
            _consumeItemFailEvent = BattleEventBus.SubscribeEvent<ConsumeItemFailEvent>(LogConsumeItemFail);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_useConsumeItemEvent);
            BattleEventBus.UnsubscribeEvent(_consumeItemFailEvent);
        }

        private void LogConsumeItem(ConsumeItemEvent consumeEvent)
        {
            var castMessage = _consumeItemMessage;
            castMessage.Add(Constants.CHARACTER_NAME, consumeEvent.Character.LocalizedName);
            castMessage.Add(Constants.ITEM_NAME, consumeEvent.ItemInfo.Data.DisplayName);
            castMessage.Add(Constants.CHARACTER_TARGET_NAME, consumeEvent.Target.LocalizedName);
            Logger.QueueLog(castMessage);
        }

        private void LogConsumeItemFail(ConsumeItemFailEvent eventObject)
        {
            Logger.AppendLog(_consumeItemFailMessage);
        }
    }
}