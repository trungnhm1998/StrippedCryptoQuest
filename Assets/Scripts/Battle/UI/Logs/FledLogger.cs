using System;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class FledLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _fledSuccessMessage;
        [SerializeField] private LocalizedString _fledUnsuccessMessage;
        private TinyMessageSubscriptionToken _fledEvent;

        private void Awake()
        {
            _fledEvent = BattleEventBus.SubscribeEvent<LogFledEvent>(OnFled);
        }

        private void OnDestroy()
        {
            BattleEventBus.UnsubscribeEvent(_fledEvent);
        }

        private void OnFled(LogFledEvent ctx)
        {
            LogFled(ctx.Character, ctx.IsSuccess);
        }

        private void LogFled(Components.Character receiver, bool isSucceeded)
        {
            LocalizedString msg = isSucceeded ? _fledSuccessMessage : _fledUnsuccessMessage;
            msg = new LocalizedString(msg.TableReference, msg.TableEntryReference)
            {
                {
                    Constants.CHARACTER_NAME, receiver.LocalizedName
                }
            };

            Logger.QueueLog(msg);
        }
    }
}