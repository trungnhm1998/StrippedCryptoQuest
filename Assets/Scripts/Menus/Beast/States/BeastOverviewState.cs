using CryptoQuest.Menus.Beast.UI;
using CryptoQuest.UI.Menu;
using FSM;

namespace CryptoQuest.Menus.Beast.States
{
    public class BeastOverviewState : StateBase
    {
        private UIBeastMenu _beastMenuPanel;

        public BeastOverviewState(UIBeastMenu beastMenuPanel) : base(false)
        {
            _beastMenuPanel = beastMenuPanel;
        }

        public override void OnEnter()
        {
            UIMainMenu.OnBackToNavigation();
            _beastMenuPanel.Input.MenuCancelEvent += HandleCancel;
        }

        public override void OnExit()
        {
            _beastMenuPanel.Input.MenuCancelEvent -= HandleCancel;
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }
    }
}
