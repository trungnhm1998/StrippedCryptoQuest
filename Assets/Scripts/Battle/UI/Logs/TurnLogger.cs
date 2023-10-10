using CryptoQuest.Battle.Events;
using System.Collections;
using System.Collections.Generic;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Battle.UI.Logs
{
    public class TurnLogger : LoggerComponentBase
    {
        private TinyMessageSubscriptionToken _skipTurnEvent;

        private void OnEnable()
        {
            _skipTurnEvent = BattleEventBus.SubscribeEvent<AbnormalEvent>(LogSkipTurn);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_skipTurnEvent);
        }

        private void LogSkipTurn(AbnormalEvent skillEvent)
        {
            var castMessage = skillEvent.Reason;
            castMessage.Add(Constants.CHARACTER_NAME, skillEvent.Character.LocalizedName);
            Logger.AppendLog(castMessage);
        }
    }
}
