using System;
using CryptoQuest.Battle.Commands;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Battle.Components.EnemyComponents.CustomBehaviours
{
    /// <summary>
    /// Enemy will not action until reach specific round
    /// </summary>
    [Serializable]
    public class NoActionUntilRound : BaseBehaviour
    {
        [SerializeField] private int _roundNumber;
        private TinyMessageSubscriptionToken _startRoundToken;
        private int _currentRound;

        public override void OnEnable(EnemyBehaviour enemyBehaviour)
        {
            base.OnEnable(enemyBehaviour);
            _startRoundToken = BattleEventBus.SubscribeEvent<RoundStartedEvent>(SetCurrentRound);
            _enemyBehaviour.TurnStarted += SetActionToNull;
        }

        public override void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_startRoundToken);
            _enemyBehaviour.TurnStarted -= SetActionToNull;
        }

        private void SetCurrentRound(RoundStartedEvent ctx)
        {
            _currentRound = ctx.Round;
        }

        private void SetActionToNull()
        {
            if (_currentRound > _roundNumber) return;

            if (!_enemyBehaviour.TryGetComponent(out CommandExecutor commandExecutor))
                return;
                
            commandExecutor.SetCommand(NullCommand.Instance);
        }
    }
}
