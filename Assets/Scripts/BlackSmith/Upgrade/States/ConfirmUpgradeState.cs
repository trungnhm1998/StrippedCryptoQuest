using CryptoQuest.BlackSmith.States;

namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public class ConfirmUpgradeState : UpgradeStateBase
    {
        public ConfirmUpgradeState(UpgradeStateMachine machine) : base(machine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            UIConfirmDetails.gameObject.SetActive(true);
            UIConfirmDetails.SetupUI(StateMachine.LevelToUpgrade, StateMachine.GoldNeeded);

            DialogsPresenter.Dialogue.Hide();
            DialogsPresenter.ShowConfirmDialog(UpgradeSystem.ConfirmUpgradeText);

            DialogsPresenter.ConfirmYesEvent += ConfirmedUpgrade;
            DialogsPresenter.ConfirmNoEvent += OnCancel;
        }

        public override void OnExit()
        {
            base.OnExit();
            UIConfirmDetails.gameObject.SetActive(false);
            DialogsPresenter.HideConfirmDialog();

            DialogsPresenter.ConfirmYesEvent -= ConfirmedUpgrade;
            DialogsPresenter.ConfirmNoEvent -= OnCancel;
        }

        public override void OnCancel() => fsm.RequestStateChange(EStates.ConfigUpgrade);

        private void ConfirmedUpgrade() => fsm.RequestStateChange(EStates.UpgradeResult);
    }
}