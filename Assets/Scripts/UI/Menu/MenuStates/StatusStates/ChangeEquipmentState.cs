using CryptoQuest.UI.Menu.Panels.Status;
using CryptoQuest.UI.Menu.Panels.Status.Equipment;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class ChangeEquipmentState : StatusStateBase
    {
        public ChangeEquipmentState(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            StatusPanel.CharacterEquipmentsPanel.Hide();
            StatusPanel.EquipmentsInventoryPanel.Show(
                StatusPanel.InspectingCharacter,
                StatusPanel.CharacterEquipmentsPanel.ModifyingSlotType);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MenuStateMachine.RequestStateChange(StatusMenuStateMachine.Equipment);
        }

        public override void OnExit()
        {
            base.OnExit();
            StatusPanel.CharacterEquipmentsPanel.Show();
            StatusPanel.EquipmentsInventoryPanel.Hide();
        }
    }
}