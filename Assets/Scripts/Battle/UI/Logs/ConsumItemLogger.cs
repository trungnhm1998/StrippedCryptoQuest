using CryptoQuest.Battle.Events;
using CryptoQuest.Item;
using System.Collections;
using System.Collections.Generic;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class ConsumItemLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _useConsumItemMessage;
        private TinyMessageSubscriptionToken _useConsumItemEvent;

        private void OnEnable()
        {
            _useConsumItemEvent = BattleEventBus.SubscribeEvent<ConsumeItemEvent>(LogConsumItem);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_useConsumItemEvent);
        }

        private void LogConsumItem(ConsumeItemEvent consumEvent)
        {
            var castMessage = _useConsumItemMessage;
            castMessage.Add(Constants.CHARACTER_NAME, consumEvent.Character.LocalizedName);
            castMessage.Add(Constants.ITEM_NAME, consumEvent.ItemInfo.Data.DisplayName);
            castMessage.Add(Constants.CHARACTER_TARGET_NAME, consumEvent.Target.LocalizedName);
            Logger.AppendLog(castMessage);
        }
    }
}
