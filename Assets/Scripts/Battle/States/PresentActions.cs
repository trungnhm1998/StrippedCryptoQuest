using CryptoQuest.Battle.Events;
using TinyMessenger;

namespace CryptoQuest.Battle.States
{
    public class PresentActions : IState
    {
        private TinyMessageSubscriptionToken _finishedPresentingActionsEvent;

        public void OnEnter(BattleStateMachine stateMachine)
        {
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
            // We already cached the result of this round in BattleResultManager
            BattleEventBus.RaiseEvent(new ChangeToCurrentResultStateEvent());
        }
    }
}