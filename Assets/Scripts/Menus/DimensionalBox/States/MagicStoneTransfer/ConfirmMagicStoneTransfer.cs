using CryptoQuest.Menus.DimensionalBox.UI.MagicStoneTransfer;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.DimensionalBox.States.MagicStoneTransfer
{
    public class ConfirmTransferMagicStoneAction : ActionBase { }

    public class ConfirmMagicStoneTransfer : StateBase
    {
        private TinyMessageSubscriptionToken _transferredEvent;
        private UIChoiceDialog _dialog;
        private TinyMessageSubscriptionToken _transferFailedEvent;

        protected override void OnEnter()
        {
            StateMachine.Input.MenuCancelEvent += OnNoButton;
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
                .WithNoCallback(OnNoButton)
                .WithYesCallback(OnYesButton)
                .SetMessage(new LocalizedString(Constants.DIMENSIONAL_BOX_TABLE_REFERENCE, Constants.CONFIRM_MESSAGE))
                .Show();
        }

        private void OnYesButton()
        {
            StateMachine.Input.MenuCancelEvent -= OnNoButton;
            _transferredEvent =
                ActionDispatcher.Bind<TransferMagicStoneSucceed>(_ => OnNoButton());
            _transferFailedEvent =
                ActionDispatcher.Bind<TransferMagicStoneFailed>(_ => OnNoButton());
            ActionDispatcher.Dispatch(new ShowLoading());

            var magicStoneController = StateMachine.MagicStoneTransferPanel.GetComponent<MagicStoneController>();

            var inGame = magicStoneController.InGameMagicStoneList.MagicStonesId;
            var dBox = magicStoneController.DBoxMagicStoneList.MagicStonesId;

            ActionDispatcher.Dispatch(new ConfirmTransferMagicStoneAction());
            ActionDispatcher.Dispatch(new SendMagicStoneToBothSide(inGame.ToArray(), dBox.ToArray()));
        }

        protected override void OnExit()
        {
            if (_transferredEvent != null) ActionDispatcher.Unbind(_transferredEvent);
            if (_transferFailedEvent != null) ActionDispatcher.Unbind(_transferFailedEvent);
            StateMachine.Input.MenuCancelEvent -= OnNoButton;
        }

        private void OnNoButton()
        {
            ChoiceDialogController.Instance.Release(_dialog);
            StateMachine.ChangeState(StateMachine.TransferringMagicStoneState);
        }
    }
}