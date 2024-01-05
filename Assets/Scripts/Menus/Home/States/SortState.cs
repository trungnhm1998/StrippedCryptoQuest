using System.Collections.Generic;
using CryptoQuest.Menus.Home.UI;
using CryptoQuest.Sagas.Party;
using FSM;
using IndiGames.Core.Events;
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
            var orderIdList = new List<int>();
            for (int i = 0; i < _panel.PartySO.GetParty().Length; i++)
            {
                if (!_panel.PartySO.GetParty()[i].Hero.IsValid()) continue;
                orderIdList.Add(_panel.PartySO.GetParty()[i].Hero.Id);
            }
            ActionDispatcher.Dispatch(new SyncPartyAction(orderIdList.ToArray()));
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