using CryptoQuest.UI.Menu.Panels.Home;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class HomeMenuStateMachine : MenuStateMachine
    {
        public static readonly string Overview = "Overview";
        public static readonly string PreSort = "PreSort";
        public static readonly string Sort = "Sort";

        private readonly UIHomeMenu _panel;

        public HomeMenuStateMachine(UIHomeMenu homeMenuPanel) : base(homeMenuPanel)
        {
            _panel = homeMenuPanel;

            // make sure the state derived from MenuStateBase
            AddState(Overview, new OverviewState(homeMenuPanel));
            AddState(PreSort, new PreSortState(homeMenuPanel));
            AddState(Sort, new SortState(homeMenuPanel));

            SetStartState(Overview);
        }
    }
}