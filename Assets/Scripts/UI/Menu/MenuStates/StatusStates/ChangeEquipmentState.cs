using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    public class ChangeEquipmentState : StatusStateBase
    {
        public ChangeEquipmentState(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            StatusPanel.CharacterEquipmentsPanel.Show(false);
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
        }
    }
}