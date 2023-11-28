using FSM;

namespace CryptoQuest.BlackSmith.State.Upgrade
{
    public class EvolveState : BlackSmithStateBase
    {
        public EvolveState(BlackSmithStateMachine stateMachine) : base(stateMachine) { }

        protected override void OnCancel()
        {
            fsm.RequestStateChange(Contants.OVERVIEW_STATE);
        }
    }
}