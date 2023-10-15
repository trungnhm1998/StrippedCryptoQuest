using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;

namespace CryptoQuest.Battle.UI.Logs
{
    public class AttackLogger : LoggerComponentBase
    {
        [SerializeField] private LocalizedString _attackLogMessage;
        private TinyMessageSubscriptionToken _normalAttackToken;

        private void OnEnable()
        {
            _normalAttackToken = BattleEventBus.SubscribeEvent<NormalAttackEvent>(LogAttack);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_normalAttackToken);
        }

        private void LogAttack(NormalAttackEvent ctx)
        {
            var attackLogMessage = _attackLogMessage;
            attackLogMessage.Add(Constants.CHARACTER_NAME, new StringVariable()
            {
                Value = ctx.Character.DisplayName
            });
            Logger.AppendLog(attackLogMessage);
        }
    }
}