using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class OverviewState : HomeStateBase
    {
        public OverviewState(UIHomeMenu homePanel) : base(homePanel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(HomePanel.TypeSO, true);
        }

        public override void ChangeTab(float direction)
        {
            base.ChangeTab(direction);
            NavigationBar.ChangeTab(direction);
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            MainMenuContext.CloseMainMenu();
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);
            NavigationBar.ChangeTab(direction.x);
        }

        public override void Interact()
        {
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.PreSort);
        }
    }
}