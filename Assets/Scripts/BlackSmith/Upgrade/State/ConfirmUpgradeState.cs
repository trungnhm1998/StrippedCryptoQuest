namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class ConfirmUpgradeState : UpgradingStateBase
    {
        private ConfirmUpgradePresenter _confirmUpgradePresenter;

        public ConfirmUpgradeState(BlackSmithStateMachine stateMachine) : base(stateMachine)
        {
            _upgradePresenter.TryGetComponent(out _confirmUpgradePresenter);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _confirmUpgradePresenter.Show();

            _confirmUpgradePresenter.ComfirmedUpgrade += ConfirmedUpgrade;
            _confirmUpgradePresenter.CancelUpgrade += OnCancel;
        }

        public override void OnExit()
        {
            base.OnExit();
            _confirmUpgradePresenter.Hide();

            RemoveEvents();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            _confirmUpgradePresenter.ComfirmedUpgrade -= ConfirmedUpgrade;
            _confirmUpgradePresenter.CancelUpgrade -= OnCancel;
        }

        protected override void OnCancel()
        {
            fsm.RequestStateChange(Contants.CONFIG_UPGRADE_STATE);
        }

        private void ConfirmedUpgrade()
        {
            _baseFSM.RequestStateChange(Contants.UPGRADE_RESULT_STATE);
        }
    }
}