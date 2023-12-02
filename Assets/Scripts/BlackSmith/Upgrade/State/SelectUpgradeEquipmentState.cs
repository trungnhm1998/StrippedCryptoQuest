using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.BlackSmith.States;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class SelectUpgradeEquipmentState : BlackSmithStateBase
    {
        private readonly EquipmentListPresenter _equipmentsPresenter;

        public SelectUpgradeEquipmentState(BlackSmithSystem context) : base(context)
        {
            context.UpgradePresenter.TryGetComponent(out _equipmentsPresenter);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            _equipmentsPresenter.EquipmentListUI.SetInteractable(true);
            _equipmentsPresenter.EquipmentListUI.ResetSelected();
            _equipmentsPresenter.ShowMessage();
            _equipmentsPresenter.OnSubmitItem += SubmitItem;
        }

        public override void OnExit()
        {
            base.OnExit();

            _equipmentsPresenter.SetInteractable(false);
            _equipmentsPresenter.OnSubmitItem -= SubmitItem;
        }

        private void SubmitItem(IUpgradeEquipment upgradeEquipment)
        {
            fsm.RequestStateChange(BlackSmith.State.CONFIG_UPGRADE);
        }

        protected override void OnCancel()
        {
            fsm.RequestStateChange(BlackSmith.State.OVERVIEW);
        }
    }
}