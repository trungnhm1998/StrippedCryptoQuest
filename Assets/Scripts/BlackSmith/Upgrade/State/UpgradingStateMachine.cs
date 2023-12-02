using FSM;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradingStateMachine : StateMachine
    {
        public UpgradingStateMachine(BlackSmithSystem context) : base(false)
        {
            AddState(BlackSmith.State.SELECT_UPGRADE, new SelectUpgradeEquipmentState(context));
            AddState(BlackSmith.State.CONFIG_UPGRADE, new ConfigUpgradeState(context));
            AddState(BlackSmith.State.CONFIRM_UPGRADE, new ConfirmUpgradeState(context));

            SetStartState(BlackSmith.State.SELECT_UPGRADE);
        }
    }
}