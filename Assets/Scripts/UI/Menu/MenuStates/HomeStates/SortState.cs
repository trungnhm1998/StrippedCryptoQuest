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
            HandleSwapDirection(direction);
        }

        /// <summary>
        /// Using the If condition instead of directly passing x-direction
        /// like this: <see cref="HomePanel.SortMode.Swap(direction.x)"/> is a must
        /// because of the appearance of a bug caused by the input system.
        /// </summary>
        private void HandleSwapDirection(Vector2 direction)
        {
            if (direction.x > 0)
                HomePanel.SortMode.SwapRight();
            else if (direction.x < 0)
                HomePanel.SortMode.SwapLeft();
        }

        public override void OnExit()
        {
            base.OnExit();
            HomePanel.SortMode.ConfirmedEvent -= ExitSortState;
        }

        private void ExitSortState()
        {
            MenuStateMachine.RequestStateChange(HomeMenuStateMachine.PreSort);
        }
    }
}