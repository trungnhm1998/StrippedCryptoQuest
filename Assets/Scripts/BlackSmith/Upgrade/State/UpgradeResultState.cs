using CryptoQuest.BlackSmith.State;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradeResultState : BlackSmithStateBase
    {
        private ResultUpgradePresenter _resultUpgradePresenter;

        public UpgradeResultState(BlackSmithStateMachine stateMachine) : base(stateMachine)
        {
            _upgradePresenter.TryGetComponent(out _resultUpgradePresenter);
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

            RemoveEvents();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            _resultUpgradePresenter.OnConfirmedResult -= ConfirmedResult;
        }

        protected override void OnCancel() { }

        private void ConfirmedResult()
        {
            fsm.RequestStateChange(Contants.UPGRADING_STATE_MACHINE);
        }
    }
}