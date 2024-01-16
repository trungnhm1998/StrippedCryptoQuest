using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.TrainingBattle.State
{
    public class TrainingStateBehaviour : BaseStateBehaviour
    {
        private TinyMessageSubscriptionToken _battleWonEvent;
        private TinyMessageSubscriptionToken _battleLostEvent;
        private TrainingBattleStateController _stateController;        
        private static readonly int WonBattle = Animator.StringToHash("WonBattleState");
        private static readonly int LoseBattle = Animator.StringToHash("LostBattleState");


        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            _battleWonEvent = BattleEventBus.SubscribeEvent<BattleWonEvent>(ChangeState);
            _battleLostEvent = BattleEventBus.SubscribeEvent<BattleLostEvent>(ExitState);
            LoadBattle();
        }

        protected override void OnExit()
        {
            BattleEventBus.UnsubscribeEvent(_battleWonEvent);
            BattleEventBus.UnsubscribeEvent(_battleLostEvent);
        }
        
        private void ExitState(BattleLostEvent ctx)
        {
            StateMachine.Play(LoseBattle);
            _stateController.IsExitState = true;
        }

        private void ChangeState(BattleEndedEvent ctx)
        {
            StateMachine.Play(WonBattle);
            _stateController.IsExitState = true;
        }

        private void LoadBattle()
        {
            BattleLoader.RequestLoadBattle(_stateController.PartyId);
        }
    }
}