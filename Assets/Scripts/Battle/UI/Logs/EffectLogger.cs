using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    /// <summary>
    /// Log effect such as poison, burn, paralyze, stun, etc.
    /// </summary>
    public class EffectLogger : LoggerComponentBase
    {
        private TinyMessageSubscriptionToken _effectAffectingMessage;
        private TinyMessageSubscriptionToken _effectAdded;
        private TinyMessageSubscriptionToken _effectRemoved;

        private void OnEnable()
        {
            _effectAdded = BattleEventBus.SubscribeEvent<EffectAddedEvent>(LogAbnormalStatus);
            _effectAffectingMessage = BattleEventBus.SubscribeEvent<EffectAffectingEvent>(LogAbnormalStatus);
            _effectRemoved = BattleEventBus.SubscribeEvent<EffectRemovedEvent>(LogAbnormalStatus);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_effectAdded);
            BattleEventBus.UnsubscribeEvent(_effectAffectingMessage);
            BattleEventBus.UnsubscribeEvent(_effectRemoved);
        }

        private void LogAbnormalStatus(EffectEvent skillEvent)
        {
            var castMessage = skillEvent.Reason;
            castMessage.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = skillEvent.Character.DisplayName
            });
            Logger.AppendLog(castMessage);
        }
    }
}