using CryptoQuest.BlackSmith.State;
using FSM;

namespace CryptoQuest.BlackSmith.Evolve.States
{
    public class EvolveStateBase : BlackSmithStateBase
    {
        protected EvolveStateMachine _evolveStateMachine;

        public EvolveStateBase(EvolveStateMachine stateMachine) : base(stateMachine.RootStateMachine)
        {
            _evolveStateMachine = stateMachine;
        }
    }
}
