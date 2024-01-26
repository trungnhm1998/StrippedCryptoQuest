using FSM;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer
{
    public class SelectEquipment : ActionState<EEquipmentState, EStateAction>
    {
        private TransferEquipmentsStateMachine _fsm;

        public SelectEquipment(TransferEquipmentsStateMachine fsm) : base(false)
        {
            _fsm = fsm;
            AddAction(EStateAction.OnCancel, () => fsm.RequestStateChange(EEquipmentState.Overview));
            AddAction(EStateAction.OnExecute, OnTransferring);
            AddAction<Vector2>(EStateAction.OnNavigate, NavigateList);
            AddAction(EStateAction.OnReset, ResetSelected);
            AddAction(EStateAction.OnInteract, ShowDetail);
        }

        private void OnTransferring()
        {
            var toWallet = _fsm.IngameList.SelectedItems;
            var toGame = _fsm.InboxList.SelectedItems;

            if (toWallet.Count == 0 && toGame.Count == 0)
            {
                Debug.Log("No item selected");
                return;
            }

            _fsm.ToWallet = toWallet;
            _fsm.ToGame = toGame;

            _fsm.IngameList.Interactable = _fsm.InboxList.Interactable = false;

            _fsm.TooltipEnabledEventChannel.RaiseEvent(false);
            fsm.RequestStateChange(EEquipmentState.Confirm);
        }

        private void NavigateList(Vector2 axis)
        {
            _fsm.TooltipEnabledEventChannel.RaiseEvent(false);
            switch (axis.x)
            {
                case 0:
                    return;
                case > 0:
                    _fsm.InboxList.TryFocus();
                    break;
                case < 0:
                    _fsm.IngameList.TryFocus();
                    break;
            }
        }

        private void ResetSelected()
        {
            _fsm.InboxList.Reset();
            _fsm.IngameList.Reset();
            _fsm.TooltipEnabledEventChannel.RaiseEvent(false);
            ActionDispatcher.Dispatch(new FetchNftEquipments());
        }

        private void ShowDetail()
        {
            _fsm.TooltipEnabledEventChannel.RaiseEvent(true);
        }

        public override void OnEnter()
        {
            if (!_fsm.IngameList.TryFocus()) _fsm.InboxList.TryFocus();
            _fsm.TooltipEnabledEventChannel.RaiseEvent(false);
        }
    }
}