using CryptoQuest.Menus.Status.UI;

namespace CryptoQuest.Menus.Status.States
{
    public class ChangeEquipmentState : StatusStateBase
    {
        public ChangeEquipmentState(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            StatusPanel.CharacterEquipmentsPanel.Hide();
            StatusPanel.EquipmentsInventoryPanel.Show();
        }

        private void HandleCancel()
        {
            fsm.RequestStateChange(StatusMenuStateMachine.Equipment);
        }

        public override void OnExit()
        {
            StatusPanel.CharacterEquipmentsPanel.Show();
            StatusPanel.EquipmentsInventoryPanel.Hide();
        }
    }
}