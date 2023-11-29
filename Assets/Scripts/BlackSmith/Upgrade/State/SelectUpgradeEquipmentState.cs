using CryptoQuest.BlackSmith.Interface;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class SelectUpgradeEquipmentState : UpgradingStateBase
    {
        private EquipmentListPresenter _equipmentsPresenter;

        public SelectUpgradeEquipmentState(BlackSmithStateMachine stateMachine) : base(stateMachine)
        {
            _upgradePresenter.TryGetComponent(out _equipmentsPresenter);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _equipmentsPresenter.EquipmentListUI.SetInteractable(true);
            _equipmentsPresenter.EquipmentListUI.ResetSelected();
            _equipmentsPresenter.ShowMessage();
            _equipmentsPresenter.OnSubmitItem += SummitedItem;
        }

        public override void OnExit()
        {
            base.OnExit();

            RemoveEvents();
        }

        protected override void RemoveEvents()
        {
            base.RemoveEvents();
            _equipmentsPresenter.SetInteractable(false);
            _equipmentsPresenter.OnSubmitItem -= SummitedItem;
        }

        private void SummitedItem(IUpgradeEquipment upgradeEquipment)
        {
            fsm.RequestStateChange(Contants.CONFIG_UPGRADE_STATE);
        }

        protected override void OnCancel()
        {
            _baseFSM.RequestStateChange(Contants.OVERVIEW_STATE);
        }
    }
}