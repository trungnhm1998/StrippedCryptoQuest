using FSM;
using CryptoQuest.BlackSmith.State.Overview;
using CryptoQuest.BlackSmith.State.Upgrade;
using CryptoQuest.BlackSmith.State;

namespace CryptoQuest.BlackSmith
{
    public static class Contants
    {
        public const string OVERVIEW_STATE = "Overview";
        public const string UPGRADE_STATE = "Upgrade";
        public const string EVOLVE_STATE = "Evolve";
    }
    
    public class BlackSmithStateMachine : StateMachine
    {
        public BlackSmithManager BlackSmithManager { get; private set; }
        public BlackSmithStateBase CurrentState { get; set; }

        public BlackSmithStateMachine(BlackSmithManager manager) : base()
        {
            BlackSmithManager = manager;

            AddState(Contants.OVERVIEW_STATE, new OverviewState(this));
            AddState(Contants.UPGRADE_STATE, new UpgradeState(this));
            AddState(Contants.EVOLVE_STATE, new EvolveState(this));

            SetStartState(Contants.OVERVIEW_STATE);
            Init();
        }
    }
}
