using System.Collections.Generic;
using CryptoQuest.Merchant;
using CryptoQuest.Sagas.Character;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
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
        [SerializeField] private MerchantInput _merchantInput;

        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");
        private TinyMessageSubscriptionToken _transferFailedEvent;
        private TinyMessageSubscriptionToken _transferSucceedEvent;

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();

            _transferSucceedEvent = ActionDispatcher.Bind<TransferSucceed>(ShowTransferSucceededMessage);
            _transferFailedEvent = ActionDispatcher.Bind<TransferFailed>(ShowTransferFailedMessage);
            _merchantInput.CancelEvent += CancelTransmission;

            _controller.UIGameList.SetInteractableAllButtons(false);
            _controller.UIDboxList.SetInteractableAllButtons(false);

            _controller.DialogsManager.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_confirmMessage)
                .Show();
        }

        protected override void OnExit()
        {
            ActionDispatcher.Unbind(_transferSucceedEvent);
            ActionDispatcher.Unbind(_transferFailedEvent);
            _merchantInput.SubmitEvent -= BackToTransferState;

            if (_controller.DialogsManager.ChoiceDialog == null) return;
            _controller.DialogsManager.ChoiceDialog.Hide();
        }

        private void ShowTransferSucceededMessage(ActionBase _) => HandleTransferMessage(_transferSucceededMsg);

        private void ShowTransferFailedMessage(ActionBase _)
        {
            Debug.LogError(_);
            ActionDispatcher.Dispatch(new ShowLoading(false));
            HandleTransferMessage(_transferFailedMsg);
        }

        private void HandleTransferMessage(LocalizedString msg)
        {
            _controller.DialogsManager.Dialogue
                .SetMessage(msg)
                .Show();

            _merchantInput.SubmitEvent += BackToTransferState;
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
            ProceedToSendCharacters();
            _controller.UICharacterReplacement.ConfirmedTransmission();
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            StateMachine.Play(CharacterReplacementState);
        }

        private void ProceedToSendCharacters()
        {
            List<int> listGameItemsToTransfer = _controller.UICharacterReplacement.SelectedGameItemsIds;
            List<int> listDboxItemsToTransfer = _controller.UICharacterReplacement.SelectedDboxItemsIds;

            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new TransferCharactersAction(listGameItemsToTransfer.ToArray(),
                listDboxItemsToTransfer.ToArray()));
        }
    }
}