using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class SortState : HomeStateBase
    {
        private UIHomeMenuSortCharacter _sortMode;

        public SortState(UIHomeMenu panel) : base(panel)
        {
            _sortMode = panel.SortMode;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(HomePanel.TypeSO);
            _sortMode.ConfirmedEvent += ExitSortState;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(HomePanel.TypeSO, true);
            _sortMode.CancelSort();
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.PreSort);
        }

        public override void Confirm()
        {
            base.Confirm();
            _sortMode.ConfirmSortOrder();
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);

            if (direction.x > 0)
                _sortMode.SwapRight();
            else if (direction.x < 0)
                _sortMode.SwapLeft();
        }

        private void ExitSortState()
        {
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.PreSort);
        }

        public override void OnExit()
        {
            base.OnExit();
            _sortMode.ConfirmSortOrder();
            _sortMode.ConfirmedEvent -= ExitSortState;
        }
    }
}