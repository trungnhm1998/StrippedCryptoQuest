using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.TrainingBattle.State
{
    public class TrainingStateBehaviour : BaseStateBehaviour
    {
        private TinyMessageSubscriptionToken _finishedPresentingEvent;
        private TrainingBattleStateController _stateController;        
        private static readonly int ExitTraining = Animator.StringToHash("ExitState");


        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            _finishedPresentingEvent = BattleEventBus.SubscribeEvent<BattleEndedEvent>(ChangeState);
            LoadBattle();
        }

        protected override void OnExit()
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingEvent);
        }

        private void ChangeState(BattleEndedEvent ctx)
        {
            StateMachine.Play(ExitTraining);
            _stateController.IsExitState = true;
        }

        private void LoadBattle()
        {
            BattleLoader.RequestLoadBattle(_stateController.PartyId);
        }
    }
}