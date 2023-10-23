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
            var useMessage = new LocalizedString(_consumeItemMessage.TableReference, 
                _consumeItemMessage.TableEntryReference)
            {
                { Constants.CHARACTER_NAME, consumeEvent.Character.LocalizedName },
                { Constants.ITEM_NAME, consumeEvent.ItemInfo.Data.DisplayName },
                { Constants.CHARACTER_TARGET_NAME, consumeEvent.Target.LocalizedName },
            };
            Logger.QueueLog(useMessage);
        }

        private void LogConsumeItemFail(ConsumeItemFailEvent eventObject)
        {
            Logger.QueueLog(_consumeItemFailMessage);
        }
    }
}