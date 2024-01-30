using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.TrainingBattle.State
{
    public class TrainingStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private ResultSO _battleResult;
        private TinyMessageSubscriptionToken _battleEndEvent;
        private TrainingBattleStateController _stateController;        
        private static readonly int WonBattle = Animator.StringToHash("WonBattleState");
        private static readonly int LoseBattle = Animator.StringToHash("LostBattleState");


        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<TrainingBattleStateController>();
            _battleEndEvent = BattleEventBus.SubscribeEvent<UnloadedBattle>(ChangeState);
            LoadBattle();
        }

        protected override void OnExit()
        {
            BattleEventBus.UnsubscribeEvent(_battleEndEvent);
        }

        private void ChangeState(UnloadedBattle ctx)
        {
            switch (_battleResult.State)
            {
                case ResultSO.EState.Win:
                    StateMachine.Play(WonBattle);
                    break;
                case ResultSO.EState.Lose:
                    StateMachine.Play(LoseBattle);
                    break;
                case ResultSO.EState.Retreat:
                    StateMachine.Play(WonBattle);
                    break;
            }

            _stateController.IsExitState = true;
        }

        private void LoadBattle()
        {
            BattleLoader.RequestLoadBattle(_stateController.PartyId);
        }
    }
}