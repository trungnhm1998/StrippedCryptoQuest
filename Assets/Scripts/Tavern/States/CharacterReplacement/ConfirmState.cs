using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class ConfirmState : StateMachineBehaviourBase
    {
        [SerializeField] private LocalizedString _confirmMessage;
        [SerializeField] private LocalizedString _transferSucceededMsg;
        [SerializeField] private LocalizedString _transferFailedMsg;

        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");
        private TinyMessageSubscriptionToken _transferFailedEvent;
        private TinyMessageSubscriptionToken _transferSucceedEvent;

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();

            _transferSucceedEvent = ActionDispatcher.Bind<TransferSucceed>(ShowTransferSucceededMessage);
            _transferFailedEvent = ActionDispatcher.Bind<TransferFailed>(ShowTransferFailedMessage);
            _controller.TavernInputManager.CancelEvent += CancelTransmission;

            _controller.UIGameList.SetInteractableAllButtons(false);
            _controller.UIWalletList.SetInteractableAllButtons(false);

            _controller.DialogsManager.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_confirmMessage)
                .Show();
        }

        protected override void OnExit()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();

            ActionDispatcher.Unbind(_transferSucceedEvent);
            ActionDispatcher.Unbind(_transferFailedEvent);
            _controller.TavernInputManager.SubmitEvent -= BackToTransferState;
        }

        private void ShowTransferSucceededMessage(ActionBase _)
        {
            HandleTransferMessage(_transferSucceededMsg);
        }

        private void ShowTransferFailedMessage(ActionBase _)
        {
            HandleTransferMessage(_transferFailedMsg);
        }

        private void HandleTransferMessage(LocalizedString msg)
        {
            _controller.DialogsManager.Dialogue
                .SetMessage(msg)
                .Show();

            _controller.TavernInputManager.SubmitEvent += BackToTransferState;
        }

        private void BackToTransferState()
        {
            _controller.DialogsManager.Dialogue.Hide();
            StateMachine.Play(CharacterReplacementState);
        }

        private void CancelTransmission()
        {
            StateMachine.Play(CharacterReplacementState);
        }

        private void YesButtonPressed()
        {
            int[] listGameItemsToTransfer = _controller.UICharacterReplacement.SelectedGameItemsIds.ToArray();
            int[] listWalletItemsToTransfer = _controller.UICharacterReplacement.SelectedWalletItemsIds.ToArray();

            ActionDispatcher.Dispatch(new ShowLoading());
            if (listGameItemsToTransfer.Length > 0)
                ActionDispatcher.Dispatch(new SendCharactersToWallet(listGameItemsToTransfer));

            if (listWalletItemsToTransfer.Length > 0)
                ActionDispatcher.Dispatch(new SendCharactersToGame(listWalletItemsToTransfer));

            _controller.UICharacterReplacement.ConfirmedTransmission();
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            StateMachine.Play(CharacterReplacementState);
        }
    }
}