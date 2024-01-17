using CryptoQuest.Battle.Events;
using TinyMessenger;

namespace CryptoQuest.Battle.States
{
    public class PresentActions : IState
    {
        private TinyMessageSubscriptionToken _finishedPresentingActionsEvent;
        private BattleStateMachine _stateMachine;

        public void OnEnter(BattleStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
            _finishedPresentingActionsEvent =
                BattleEventBus.SubscribeEvent<FinishedPresentingActionsEvent>(ToNextState);
            BattleEventBus.RaiseEvent(new StartPresentingEvent());
        }

        public void OnExit(BattleStateMachine stateMachine)
        {
            BattleEventBus.UnsubscribeEvent(_finishedPresentingActionsEvent);
        }

        private void ToNextState(FinishedPresentingActionsEvent _)
        {
            /*
             * We already cached the result of this round in
             * - HandleWon
             * - HandleLost
             * - HandleRetreat
             *
             * This will tell one of to end the battle if cached result is not null. When this happens, PostRoundHandler will
             * prevent the state machine from changing state to SelectHeroesActions.
             * PostRoundHandler will change the state to SelectHeroesActions if only cached result is null.
             */
            BattleEventBus.RaiseEvent(new FinishedPresentingEvent());
            _stateMachine.ChangeState(_stateMachine.ResultChecker);
        }
    }
}