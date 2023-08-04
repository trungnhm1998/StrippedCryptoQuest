using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class ChangeEquipmentState : StatusStateBase
    {
        private readonly UIStatusInventory _inventoryPanel;

        public ChangeEquipmentState(UIStatusMenu statusPanel) : base(statusPanel)
        {
            _inventoryPanel = statusPanel.InventoryPanel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _inventoryPanel.Show(StatusPanel.EquippingType);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.Equipment);
        }

        public override void OnExit()
        {
            base.OnExit();
            _inventoryPanel.Hide();
            StatusPanel.EquippingType = UIEquipmentSlotButton.EEquipmentType.None;
        }
    }
}