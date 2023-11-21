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
            _homePanel.Focusing += ToSelectActions;
        }

        public override void OnExit()
        {
            _homePanel.Input.MenuCancelEvent -= HandleCancel;
            _homePanel.Focusing -= ToSelectActions;
            _homePanel.UIOverview.ChangeOrderEvent -= ToSorting;
            _homePanel.UIOverview.ViewCharacterListEvent -= ToCharacterList;
        }

        private void HandleCancel()
        {
            UIMainMenu.OnBackToNavigation();
        }

        private void ToSelectActions()
        {
            _homePanel.UIOverview.EnableSelectActions();
            _homePanel.UIOverview.ChangeOrderEvent += ToSorting;
            _homePanel.UIOverview.ViewCharacterListEvent += ToCharacterList;
        }

        private void ToCharacterList() => fsm.RequestStateChange(HomeMenuStateMachine.CharacterList);

        private void ToSorting() => fsm.RequestStateChange(HomeMenuStateMachine.PreSort);
    }
}