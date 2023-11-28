using CryptoQuest.Menus.Home.UI;
using CryptoQuest.UI.Menu;
using FSM;

namespace CryptoQuest.Menus.Home.States
{
    public class OverviewState : StateBase
    {
        private UIHomeMenu _homePanel;
        public OverviewState(UIHomeMenu homePanel) : base(false)
        {
            _homePanel = homePanel;
        }

        public override void OnEnter()
        {
            UIMainMenu.OnBackToNavigation();
            _homePanel.Input.MenuCancelEvent += HandleCancel;
            _homePanel.Input.MenuInteractEvent += ToSorting;
            _homePanel.Focusing += ToSorting;
        }

        public override void OnExit()
        {
            _homePanel.Input.MenuCancelEvent -= HandleCancel;
            _homePanel.Input.MenuInteractEvent -= ToSorting;
            _homePanel.Focusing -= ToSorting;
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }

        private void ToSorting() => fsm.RequestStateChange(HomeMenuStateMachine.PreSort);
    }
}