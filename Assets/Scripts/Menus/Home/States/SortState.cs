using CryptoQuest.Menus.Home.UI;
using CryptoQuest.UI.Menu;
using FSM;
using UnityEngine;

namespace CryptoQuest.Menus.Home.States
{
    public class SortState : StateBase
    {
        private UIHomeMenu _panel;

        public SortState(UIHomeMenu panel) : base(false)
        {
            _panel = panel;
        }

        public override void OnEnter()
        {
            UIMainMenu.OnFocusTab(0);
            _panel.SortMode.ConfirmedEvent += ExitSortState;
            _panel.Input.MenuCancelEvent += HandleCancel;
            _panel.Input.MenuConfirmedEvent += Confirm;
            _panel.Input.MenuNavigateEvent += HandleNavigate;
        }

        public override void OnExit()
        {
            _panel.SortMode.ConfirmedEvent -= ExitSortState;
            _panel.Input.MenuCancelEvent -= HandleCancel;
            _panel.Input.MenuConfirmedEvent -= Confirm;
            _panel.Input.MenuNavigateEvent -= HandleNavigate;
        }

        private void ExitSortState()
        {
            fsm.RequestStateChange(HomeMenuStateMachine.PreSort);
        }

        private void HandleCancel()
        {
            _panel.SortMode.CancelSort();
            fsm.RequestStateChange(HomeMenuStateMachine.PreSort);
        }

        private void Confirm()
        {
            _panel.SortMode.ConfirmSortOrder();
        }

        /// <summary>
        /// Using the If condition instead of directly passing x-direction
        /// like this: <see cref="_panel.SortMode.Swap(direction.x)"/> is a must
        /// because of the appearance of a bug caused by the input system.
        /// </summary>
        private void HandleNavigate(Vector2 direction)
        {
            switch (direction.x)
            {
                case > 0:
                    _panel.SortMode.SwapRight();
                    break;
                case < 0:
                    _panel.SortMode.SwapLeft();
                    break;
            }
        }
    }
}