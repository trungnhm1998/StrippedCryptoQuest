using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.States
{
    public class SelectUpgradeEquipmentState : UpgradeStateBase
    {
        public SelectUpgradeEquipmentState(UpgradeStateMachine machine) : base(machine) { }

        public override void OnEnter()
        {
            base.OnEnter();

            EquipmentsPresenter.gameObject.SetActive(true);
            DialogsPresenter.Dialogue.SetMessage(UpgradeSystem.SelectEquipmentToUpgradeText).Show();
            EquipmentsPresenter.OnSubmitItem += SubmitItem;
        }

        public override void OnExit()
        {
            base.OnExit();

            EquipmentsPresenter.SetInteractable(false);
            EquipmentsPresenter.OnSubmitItem -= SubmitItem;
        }

        private void SubmitItem(IEquipment upgradeEquipment)
        {
            StateMachine.EquipmentToUpgrade = upgradeEquipment;
            fsm.RequestStateChange(EStates.ConfigUpgrade);
        }

        public override void OnCancel()
        {
            StateMachine.BackToOverview();
        }
    }
}