using FSM;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradingStateMachine : StateMachine
    {
        private EquipmentListPresenter _equipmentsPresenter;
        private UpgradePresenter _upgradePresenter;
        private readonly BlackSmithManager _manager;
        private readonly BlackSmithStateMachine _baseFSM;

        public UpgradingStateMachine(BlackSmithStateMachine stateMachine) : base(false)
        {
            _baseFSM = stateMachine;
            _manager = _baseFSM.BlackSmithManager;
            _upgradePresenter = _manager.UpgradePresenter;
            _upgradePresenter.TryGetComponent<EquipmentListPresenter>(out _equipmentsPresenter);

            AddState(Contants.SELECT_UPGRADE_STATE, new SelectUpgradeEquipmentState(stateMachine));
            AddState(Contants.CONFIG_UPGRADE_STATE, new ConfigUpgradeState(stateMachine));
            AddState(Contants.CONFIRM_UPGRADE_STATE, new ConfirmUpgradeState(stateMachine));

            SetStartState(Contants.SELECT_UPGRADE_STATE);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            RequestStateChange(Contants.SELECT_UPGRADE_STATE);
            _equipmentsPresenter.Show();
        }

        public override void OnExit()
        {
            base.OnExit();
            _equipmentsPresenter.Hide();
        }
    }
}