using CryptoQuest.BlackSmith.States;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class ConfirmUpgradeState : BlackSmithStateBase
    {
        private readonly ConfirmUpgradePresenter _confirmUpgradePresenter;

        public ConfirmUpgradeState(BlackSmithSystem context) : base(context)
        {
            context.UpgradePresenter.TryGetComponent(out _confirmUpgradePresenter);
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

            _confirmUpgradePresenter.ComfirmedUpgrade -= ConfirmedUpgrade;
            _confirmUpgradePresenter.CancelUpgrade -= OnCancel;
        }

        protected override void OnCancel() => fsm.RequestStateChange(BlackSmith.State.CONFIG_UPGRADE);

        private void ConfirmedUpgrade() => fsm.RequestStateChange(BlackSmith.State.UPGRADE_RESULT);
    }
}