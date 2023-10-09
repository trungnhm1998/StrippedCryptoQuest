using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

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
            attackLogMessage.Add(Constants.CHARACTER_NAME, ctx.Character.LocalizedName);
            Logger.AppendLog(attackLogMessage);
        }
    }
}