using CryptoQuest.UI.Menu.Panels.Home;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class OverviewState : MenuStateBase
    {
        protected UIHomeMenu HomePanel { get; }

        public OverviewState(UIHomeMenu homePanel)
        {
            HomePanel = homePanel;
        }

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
        }
    }
}