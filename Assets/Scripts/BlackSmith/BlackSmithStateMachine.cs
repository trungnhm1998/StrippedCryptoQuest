using FSM;
using CryptoQuest.BlackSmith.State.Overview;
using CryptoQuest.BlackSmith.State.Upgrade;
using CryptoQuest.BlackSmith.State;
using CryptoQuest.BlackSmith.Upgrade.State;

namespace CryptoQuest.BlackSmith
{
    public static class Contants
    {
        public const string OVERVIEW_STATE = "Overview";
        public const string UPGRADING_STATE_MACHINE = "Upgrading";
        public const string SELECT_UPGRADE_STATE = "SelectUpgrade";
        public const string CONFIG_UPGRADE_STATE = "ConfigUpgrade";
        public const string CONFIRM_UPGRADE_STATE = "ConfirmUpgrade";
        public const string UPGRADE_RESULT_STATE = "UpgradeResult";
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
            AddState(Contants.UPGRADING_STATE_MACHINE, new UpgradingStateMachine(this));
            AddState(Contants.UPGRADE_RESULT_STATE, new UpgradeResultState(this));
            AddState(Contants.EVOLVE_STATE, new EvolveState(this));

            SetStartState(Contants.OVERVIEW_STATE);
            Init();
        }
    }
}
