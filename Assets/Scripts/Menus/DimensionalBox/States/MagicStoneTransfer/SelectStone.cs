using CryptoQuest.Sagas.MagicStone;
using FSM;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer
{
    public class SelectStone : ActionState<EMagicStoneState, EStateAction>
    {
        private TransferringMagicStoneStateMachine _fsm;

        public SelectStone(TransferringMagicStoneStateMachine fsm) : base(false)
        {
            _fsm = fsm;

            AddAction(EStateAction.OnCancel, OnCancel);
            AddAction(EStateAction.OnExecute, OnTransferring);
            AddAction<Vector2>(EStateAction.OnNavigate, NavigateList);
            AddAction(EStateAction.OnReset, ResetSelected);
        }


        public override void OnEnter()
        {
            if (!_fsm.IngameList.TryFocus()) _fsm.DBoxList.TryFocus();
        }

        private void OnTransferring()
        {
            var toWallet = _fsm.IngameList.SelectedItems;
            var toGame = _fsm.DBoxList.SelectedItems;

            if (toWallet.Count == 0 && toGame.Count == 0)
            {
                Debug.Log("No item selected");
                return;
            }

            _fsm.ToWallet = toWallet;
            _fsm.ToGame = toGame;

            _fsm.IngameList.Interactable = _fsm.DBoxList.Interactable = false;

            fsm.RequestStateChange(EMagicStoneState.Confirm);
        }

         private void NavigateList(Vector2 axis)
        {
            switch (axis.x)
            {
                case 0:
                    return;
                case > 0:
                    _fsm.DBoxList.TryFocus();
                    break;
                case < 0:
                    _fsm.IngameList.TryFocus();
                    break;
            }
        }        private void ResetSelected()
        {
            _fsm.DBoxList.Reset();
            _fsm.IngameList.Reset();
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
        }

        private void OnCancel() => fsm.RequestStateChange(EMagicStoneState.Overview);
    }
}