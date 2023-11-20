using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.AbilitySystem.Executions;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class MissedLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _missedMessage;
        [SerializeField] private LocalizedString _noDamageMessage;
        private TinyMessageSubscriptionToken _missedEvent;

        private void Awake()
        {
            _missedEvent = BattleEventBus.SubscribeEvent<MissedEvent>(LogMissed);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_missedEvent);
        }
        
        private void LogMissed(MissedEvent ctx)
        {
            Logger.QueueLog(_missedMessage);

            if (ctx.IsDamage)
                LogNoDamage(ctx.Character);
        }

        private void LogNoDamage(Components.Character receiver)
        {
            LocalizedString msg =
                new LocalizedString(_noDamageMessage.TableReference, _noDamageMessage.TableEntryReference);

            msg.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = receiver.DisplayName
            });

            Logger.QueueLog(msg);
        }
    }
}