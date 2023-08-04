using CryptoQuest.UI.Menu.Panels.Home;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class HomeMenuStateMachine : MenuStateMachine
    {
        public static readonly string Overview = "Overview";
        public static readonly string SelectSort = "SelectSort";
        public static readonly string Sorting = "Sorting";

        private readonly UIHomeMenu _panel;

        public HomeMenuStateMachine(UIHomeMenu homeMenuPanel) : base(homeMenuPanel)
        {
            _panel = homeMenuPanel;

            // make sure the state derived from MenuStateBase
            AddState(Overview, new OverviewState(homeMenuPanel));
            // AddState(SelectSort, new State(onLogic: Logic));
            // AddState(Sorting, new State(onLogic: Logic));

            SetStartState(Overview);
        }
    }
}