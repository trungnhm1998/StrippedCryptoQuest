using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.DimensionalBox.States.EquipmentsTransfer
{
    public class ConfirmTransferAction : ActionBase { }

    internal class ConfirmTransfer : StateBase
    {
        private TinyMessageSubscriptionToken _transferredEvent;
        private UIChoiceDialog _dialog;
        private TinyMessageSubscriptionToken _transferFailedEvent;

        protected override void OnEnter()
        {
            StateMachine.Input.MenuCancelEvent += BackToSelectEquipmentsToTransfer;
            if (_dialog == null)
            {
                ChoiceDialogController.Instance.Instantiate(ShowConfirmDialog);
                return;
            }

            ShowConfirmDialog(_dialog);
        }

        private void ShowConfirmDialog(UIChoiceDialog dialog)
        {
            _dialog = dialog;
            _dialog
                .WithNoCallback(BackToSelectEquipmentsToTransfer)
                .WithYesCallback(() =>
                {
                    StateMachine.Input.MenuCancelEvent -= BackToSelectEquipmentsToTransfer;
                    _transferredEvent = ActionDispatcher.Bind<TransferSucceed>(_ => BackToSelectEquipmentsToTransfer());
                    _transferFailedEvent =
                        ActionDispatcher.Bind<TransferFailed>(_ => BackToSelectEquipmentsToTransfer());
                    ActionDispatcher.Dispatch(new ShowLoading());
                    ActionDispatcher.Dispatch(new ConfirmTransferAction());
                })
                .SetMessage(new LocalizedString(Constants.DIMENSIONAL_BOX_TABLE_REFERENCE, Constants.CONFIRM_MESSAGE))
                .Show();
        }

        protected override void OnExit()
        {
            if (_transferredEvent != null) ActionDispatcher.Unbind(_transferredEvent);
            if (_transferFailedEvent != null) ActionDispatcher.Unbind(_transferFailedEvent);
            StateMachine.Input.MenuCancelEvent -= BackToSelectEquipmentsToTransfer;
        }

        private void BackToSelectEquipmentsToTransfer()
        {
            ChoiceDialogController.Instance.Release(_dialog);
            StateMachine.ChangeState(StateMachine.TransferringEquipmentsState);
        }
    }
}