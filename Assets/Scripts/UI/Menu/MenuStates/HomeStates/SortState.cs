using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public class SortState : HomeStateBase
    {
        public SortState(UIHomeMenu panel) : base(panel) { }

        public override void OnEnter()
        {
            base.OnEnter();
            NavigationBar.SetActive(false);
            NavigationBar.HighlightHeader(HomePanel.TypeSO);
            HomePanel.SortMode.ConfirmedEvent += ExitSortState;
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            NavigationBar.SetActive(true);
            NavigationBar.HighlightHeader(HomePanel.TypeSO, true);
            HomePanel.SortMode.CancelSort();
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.PreSort);
        }

        public override void Confirm()
        {
            base.Confirm();
            HomePanel.SortMode.ConfirmSortOrder();
        }

        public override void HandleNavigate(Vector2 direction)
        {
            base.HandleNavigate(direction);
            // HomePanel.SortMode.Swap(direction.x);

            if (direction.x > 0)
                HomePanel.SortMode.SwapRight();
            else if (direction.x < 0)
                HomePanel.SortMode.SwapLeft();
        }

        private void ExitSortState()
        {
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.PreSort);
        }

        public override void OnExit()
        {
            base.OnExit();
            HomePanel.SortMode.ConfirmSortOrder();
            HomePanel.SortMode.ConfirmedEvent -= ExitSortState;
        }
    }
}