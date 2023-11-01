using CryptoQuest.Menus.Home.UI;
using CryptoQuest.UI.Menu;
using FSM;

namespace CryptoQuest.Menus.Home.States
{
    public class PreSortState : StateBase
    {
        private UIHomeMenu _panel;

        public PreSortState(UIHomeMenu panel) : base(false)
        {
            _panel = panel;
        }

        public override void OnEnter()
        {
            UIMainMenu.OnFocusTab(0);
            _panel.Input.MenuCancelEvent += HandleCancel;
            _panel.SortMode.SelectedEvent += StartSorting;
            _panel.SortMode.Init();
        }

        private void HandleCancel()
        {
            _panel.SortMode.DeInit();
            _panel.SortMode.SetDefaultSelection();
            fsm.RequestStateChange(HomeMenuStateMachine.Overview);
        }

        private void StartSorting()
        {
            fsm.RequestStateChange(HomeMenuStateMachine.Sort);
        }

        public override void OnExit()
        {
            _panel.SortMode.DeInit();
            _panel.Input.MenuCancelEvent -= HandleCancel;
            _panel.SortMode.SelectedEvent -= StartSorting;
        }
    }
}