namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public class UpgradeResultState : UpgradeStateBase
    {
        public UpgradeResultState(UpgradeStateMachine machine) : base(machine) { }

        public override void OnEnter()
        {
            base.OnEnter();
            CurrencyPresenter.Show();
            ResultUI.SetActive(true);
            EquipmentsPresenter.gameObject.SetActive(false);
            DialogsPresenter.Dialogue.SetMessage(UpgradeSystem.UpgradeResultText).Show();
        }

        public override void OnExit()
        {
            base.OnExit();
            ResultUI.SetActive(false);
        }

        public override void OnCancel() => BackToUpgrade();

        public override void OnSubmit() => BackToUpgrade();

        private void BackToUpgrade()
        {
            fsm.RequestStateChange(EStates.SelectEquipment);
        }
    }
}