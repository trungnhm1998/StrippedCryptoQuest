using IndiGames.Core.Events;
using TinyMessenger;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class ConfirmTransfer : TransferMetadBaseState
    {
        private TinyMessageSubscriptionToken _transferSucceedEvent;
        private TinyMessageSubscriptionToken _transferFailedEvent;

        private bool _isTransferring;
    
        public ConfirmTransfer(TransferMetadStateMachine fsm) : base(fsm)
        {
            AddAction(EStateAction.OnCancel, BackToInputAmound);
            AddAction(EStateAction.OnReset, BackToInputAmound);
        }
    
        public override void OnEnter()
        {
            _panel.SetInteractable(false);
            _panel.ConfirmDialog.ShowConfirmDialog();
            _panel.ConfirmDialog.ConfirmNoEvent += BackToInputAmound;
            _panel.ConfirmDialog.ConfirmYesEvent += DispatchTransferMetad;

            _transferSucceedEvent = ActionDispatcher.Bind<TransferringMetadSuccess>(_ => BackToSelectSource());
            _transferFailedEvent = ActionDispatcher.Bind<TransferringMetadFailed>(ShowErrorDialogAndBackToSelectSource);
        }
    
        public override void OnExit()
        {
            _panel.ConfirmDialog.ConfirmNoEvent -= BackToInputAmound;
            _panel.ConfirmDialog.ConfirmYesEvent -= DispatchTransferMetad;
            _panel.ConfirmDialog.HideConfirmDialog();
            ActionDispatcher.Unbind(_transferSucceedEvent);
            ActionDispatcher.Unbind(_transferFailedEvent);
        }
    
        private void BackToInputAmound()
        {
            if (_isTransferring) return;
            _fsm.RequestStateChange(EMetadState.InputTransferAmount);
        }
    
        private void BackToSelectSource()
        {
            _isTransferring = false;
            _fsm.RequestStateChange(EMetadState.SelectSource);
        }
    
        private void DispatchTransferMetad()
        {
            _isTransferring = true;
            ActionDispatcher.Dispatch(new TransferringMetad(_fsm.SelectedCurrency, _fsm.TransferAmount));
        }
    
        private void ShowErrorDialogAndBackToSelectSource(TransferringMetadFailed _)
        {
            BackToSelectSource();
        }
    }
}