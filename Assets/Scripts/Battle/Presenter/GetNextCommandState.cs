using CryptoQuest.Battle.Events;

namespace CryptoQuest.Battle.Presenter
{
    public class GetNextCommandState : StateBase
    {
        protected override void OnEnter()
        {
            base.OnEnter();
            if (StateMachine.Commands.Count > 0)
                StateMachine.ChangeState(StateMachine.Commands.Dequeue().GetState());
            else
            {
                BattleEventBus.RaiseEvent(new FinishedPresentingActionsEvent());
                StateMachine.ChangeState(StateMachine.Hide); // TODO?: Make sure no commands are enqueued after Hide
            }
        }
    }
}