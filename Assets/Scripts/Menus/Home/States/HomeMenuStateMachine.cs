using CryptoQuest.Menus.Home.UI;
using FSM;

namespace CryptoQuest.Menus.Home.States
{
    public class HomeMenuStateMachine : StateMachine
    {
        public static readonly string Overview = "Overview";
        public static readonly string PreSort = "PreSort";
        public static readonly string Sort = "Sort";

        private readonly UIHomeMenu _panel;

        public HomeMenuStateMachine(UIHomeMenu homeMenuPanel) : base(false)
        {
            _panel = homeMenuPanel;

            AddState(Overview, new OverviewState(homeMenuPanel));
            AddState(PreSort, new PreSortState(homeMenuPanel));
            AddState(Sort, new SortState(homeMenuPanel));

            SetStartState(Overview);
        }
    }
}