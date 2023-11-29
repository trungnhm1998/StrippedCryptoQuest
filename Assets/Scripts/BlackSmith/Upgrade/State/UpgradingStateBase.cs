using CryptoQuest.BlackSmith.State;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradingStateBase : BlackSmithStateBase
    {
        protected readonly BlackSmithStateMachine _baseFSM;

        public UpgradingStateBase(BlackSmithStateMachine stateMachine) : base(stateMachine)
        {
            _baseFSM = stateMachine;
        }
    }
}