using CryptoQuest.PushdownFSM;
using CryptoQuest.UI.Battle.StateMachine;
using FSM;

namespace CryptoQuest.UI.Battle.SelectCommand
{
    public class SelectCommandState : PushdownStateBase
    {
        public SelectCommandState(IPushdownStateMachine<string, StateBase> stateMachine)
         : base(stateMachine) { }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}