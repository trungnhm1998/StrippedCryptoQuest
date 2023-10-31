using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Loot;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class StealLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _stealSuccessMessage;
        [SerializeField] private LocalizedString _stealFailedMessage;
        private TinyMessageSubscriptionToken _stealSuccessEvent;
        private TinyMessageSubscriptionToken _stealFailedEvent;

        private void Awake()
        {
            _stealSuccessEvent = BattleEventBus.SubscribeEvent<StealSuccessEvent>(LogStealSuccess);
            _stealFailedEvent = BattleEventBus.SubscribeEvent<StealFailedEvent>(LogStealFail);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_stealSuccessEvent);
            BattleEventBus.UnsubscribeEvent(_stealFailedEvent);
        }
        
        private void LogStealSuccess(StealSuccessEvent ctx)
        {
            var log = CreateLogWithCharacterName(_stealSuccessMessage,
                ctx.Source.LocalizedName, ctx.Target.LocalizedName);
            log.Add(Constants.ITEM_NAME, new StringVariable()
            {
                Value = ctx.StealableItem.DisplayName.GetLocalizedString(),
            });

            Logger.QueueLog(log);
        }

        private void LogStealFail(StealFailedEvent ctx)
        {
            var log = CreateLogWithCharacterName(_stealFailedMessage,
                ctx.Source.LocalizedName, ctx.Target.LocalizedName);

            Logger.QueueLog(log);
        }

        private LocalizedString CreateLogWithCharacterName(LocalizedString inputMessage,
            LocalizedString sourceName, LocalizedString targetName)
        {
            var outMessage = new LocalizedString(inputMessage.TableReference,
                inputMessage.TableEntryReference);

            outMessage.Add(Constants.CHARACTER_NAME, sourceName);
            outMessage.Add(Constants.CHARACTER_TARGET_NAME, targetName);

            return outMessage;
        }
    }
}