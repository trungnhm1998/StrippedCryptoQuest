using CryptoQuest.Sagas.MagicStone;
using FSM;
using IndiGames.Core.Events;
using UnityEngine;

namespace CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer
{
    public class SelectStone : ActionState<EMagicStoneState, EStateAction>
    {
        private TransferringMagicStoneStateMachine _fsm;
        private bool _hasDetailOpened;

        public SelectStone(TransferringMagicStoneStateMachine fsm) : base(false)
        {
            _fsm = fsm;

            AddAction(EStateAction.OnCancel, OnCancel);
            AddAction(EStateAction.OnExecute, OnTransferring);
            AddAction<Vector2>(EStateAction.OnNavigate, NavigateList);
            AddAction(EStateAction.OnReset, ResetSelected);
            AddAction(EStateAction.OnInteract, ToggleStoneDetailVisibility);
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
            HideStoneTooltip();

            fsm.RequestStateChange(EMagicStoneState.Confirm);
        }

         private void NavigateList(Vector2 axis)
        {
            HideStoneTooltip();
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
        }        
        
        private void ResetSelected()
        {
            HideStoneTooltip();
            _fsm.DBoxList.Reset();
            _fsm.IngameList.Reset();
            ActionDispatcher.Dispatch(new FetchProfileMagicStonesAction());
        }
        
        private void ToggleStoneDetailVisibility()
        {
            _hasDetailOpened = !_hasDetailOpened;
            _fsm.ShowTooltipEventChannel.RaiseEvent(_hasDetailOpened);
        }

        private void HideStoneTooltip()
        {
            if(!_hasDetailOpened) return;
            _hasDetailOpened = false;
            _fsm.ShowTooltipEventChannel.RaiseEvent(_hasDetailOpened);
        }

        private void OnCancel() 
        {
            HideStoneTooltip();
            fsm.RequestStateChange(EMagicStoneState.Overview);
        }
    }
}