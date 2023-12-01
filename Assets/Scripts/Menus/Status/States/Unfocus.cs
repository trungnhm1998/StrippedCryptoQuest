using CryptoQuest.Menus.Status.UI;
using CryptoQuest.UI.Menu;

namespace CryptoQuest.Menus.Status.States
{
    public class Unfocus : StatusStateBase
    {
        public Unfocus(UIStatusMenu statusPanel) : base(statusPanel) { }

        public override void OnEnter()
        {
            StatusPanel.Focusing += FocusSelectEquipmentPanel;
            UIMainMenu.OnBackToNavigation();
        }

        public override void OnExit()
        {
            StatusPanel.Focusing -= FocusSelectEquipmentPanel;
        }

        private void FocusSelectEquipmentPanel() => fsm.RequestStateChange(State.OVERVIEW);
    }
}