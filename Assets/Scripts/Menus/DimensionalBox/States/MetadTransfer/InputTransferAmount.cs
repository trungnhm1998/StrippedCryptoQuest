using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.OneButtonDialog;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.DimensionalBox.States.MetadTransfer
{
    public class InputTransferAmount : StateBase
    {
        private readonly UIMetadTransferPanel _metaDTransferPanel;
        private TinyMessageSubscriptionToken _transferSucceedEvent;
        private TinyMessageSubscriptionToken _transferFailedEvent;

        public InputTransferAmount(GameObject transferPanel)
        {
            _metaDTransferPanel = transferPanel.GetComponent<UIMetadTransferPanel>();
        }

        protected override void OnEnter()
        {
            _metaDTransferPanel.TransferAmountInput.Select();
            StateMachine.Input.MenuCancelEvent += ToSelectTransferTypeState;
            StateMachine.Input.MenuExecuteEvent += ConfirmTransfer;
            _transferSucceedEvent = ActionDispatcher.Bind<TransferringMetadSuccess>(_ => ToSelectTransferTypeState());
            _transferFailedEvent = ActionDispatcher.Bind<TransferringMetadFailed>(ShowErrorDialogAndBackToSelectSource);
        }

        protected override void OnExit()
        {
            _metaDTransferPanel.TransferAmountInput.text = "";
            StateMachine.Input.MenuCancelEvent -= ToSelectTransferTypeState;
            StateMachine.Input.MenuExecuteEvent -= ConfirmTransfer;
            ActionDispatcher.Unbind(_transferSucceedEvent);
            ActionDispatcher.Unbind(_transferFailedEvent);
        }

        private void ToSelectTransferTypeState()
        {
            StateMachine.ChangeState(StateMachine.TransferringMetaDState);
        }

        private void ConfirmTransfer()
        {
            if (string.IsNullOrEmpty(_metaDTransferPanel.TransferAmountInput.text)) return;
            StateMachine.Input.MenuExecuteEvent -= ConfirmTransfer; // prevent pressing confirm while transferring
            ChoiceDialogController.Instance.Instantiate(ShowConfirmDialog);
        }

        private void ShowConfirmDialog(UIChoiceDialog dialog)
        {
            dialog
                .SetMessage(new LocalizedString(Constants.DIMENSIONAL_BOX_TABLE_REFERENCE, Constants.CONFIRM_MESSAGE))
                .WithYesCallback(DispatchTransferMetaD)
                .WithNoCallback(() =>
                {
                    _metaDTransferPanel.TransferAmountInput.Select();
                    ChoiceDialogController.Instance.Release(dialog);
                })
                .Show();
        }

        private void DispatchTransferMetaD()
        {
            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new TransferringMetad(_metaDTransferPanel.SourceToTransfer,
                float.Parse(_metaDTransferPanel.TransferAmountInput.text)));
        }

        private void ShowErrorDialogAndBackToSelectSource(TransferringMetadFailed _)
        {
            GenericOneButtonDialogController
                .Instance.Instantiate(ShowErrorDialog);
        }

        private void ShowErrorDialog(UIOneButtonDialog dialog)
        {
            dialog
                .WithMessage(new LocalizedString(Constants.DIMENSIONAL_BOX_TABLE_REFERENCE, Constants.ERROR))
                .WithButtonCallback(ToSelectTransferTypeState)
                .Show();
        }
    }
}