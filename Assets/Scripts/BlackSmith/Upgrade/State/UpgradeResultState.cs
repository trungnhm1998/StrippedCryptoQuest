using CryptoQuest.BlackSmith.States;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradeResultState : BlackSmithStateBase
    {
        private readonly ResultUpgradePresenter _resultUpgradePresenter;

        public UpgradeResultState(BlackSmithSystem context) : base(context)
        {
            context.UpgradePresenter.TryGetComponent(out _resultUpgradePresenter);
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _resultUpgradePresenter.Show();

            _resultUpgradePresenter.OnConfirmedResult += ConfirmedResult;
        }

        public override void OnExit()
        {
            base.OnExit();
            _resultUpgradePresenter.Hide();

            _resultUpgradePresenter.OnConfirmedResult -= ConfirmedResult;
        }

        protected override void OnCancel() => BackToUpgrade();

        private void ConfirmedResult() => BackToUpgrade();

        private void BackToUpgrade()
        {
            fsm.RequestStateChange(BlackSmith.State.UPGRADING);
        }
    }
}