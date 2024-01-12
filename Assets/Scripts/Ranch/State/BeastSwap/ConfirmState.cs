using CryptoQuest.Ranch.UI;
using CryptoQuest.UI.Actions;
using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch.State.BeastSwap
{
    public class ConfirmState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _confirmMessage;
        [SerializeField] private LocalizedString _transferSucceededMsg;
        [SerializeField] private LocalizedString _transferFailedMsg;

        private RanchStateController _controllerState;

        private static readonly int SwapState = Animator.StringToHash("SwapState");

        private TinyMessageSubscriptionToken _transferFailedEvent;
        private TinyMessageSubscriptionToken _transferSucceedEvent;

        protected override void OnEnter()
        {
            _controllerState = StateMachine.GetComponent<RanchStateController>();

            _transferSucceedEvent = ActionDispatcher.Bind<TransferSucceed>(ShowTransferSucceededMessage);
            _transferFailedEvent = ActionDispatcher.Bind<TransferFailed>(ShowTransferFailedMessage);

            _controllerState.Controller.Input.CancelEvent += CancelTransmission;

            _controllerState.Controller.DialogController.ChoiceDialog
                .SetButtonsEvent(OnConfirm, OnCancel)
                .SetMessage(_confirmMessage)
                .Show();
        }

        protected override void OnExit()
        {
            _controllerState.Controller.Input.SubmitEvent -= BackToSwapState;

            ActionDispatcher.Unbind(_transferSucceedEvent);
            ActionDispatcher.Unbind(_transferFailedEvent);

            _controllerState.Controller.DialogController.ChoiceDialog.Hide();
        }

        private void CancelTransmission() => StateMachine.Play(SwapState);

        private void OnConfirm() => ProceedToSendBeasts();

        private void ShowTransferFailedMessage(ActionBase _) => HandleTransferMessage(_transferFailedMsg);

        private void ShowTransferSucceededMessage(ActionBase _)
        {
            ActionDispatcher.Dispatch(new ShowLoading(false));
            HandleTransferMessage(_transferSucceededMsg);
        }

        private void OnCancel()
        {
            _controllerState.Controller.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(SwapState);
        }

        private void HandleTransferMessage(LocalizedString msg)
        {
            _controllerState.Controller.DialogController.NormalDialogue
                .SetMessage(msg)
                .Show();

            _controllerState.Controller.Input.SubmitEvent += BackToSwapState;
        }

        private void BackToSwapState()
        {
            _controllerState.Controller.DialogController.NormalDialogue.Hide();
            StateMachine.Play(SwapState);
        }

        private void ProceedToSendBeasts()
        {
            UIBeastItem[] inGameBeasts = _controllerState.UIBeastSwap.ToGame.ToArray();
            UIBeastItem[] inDBoxBeasts = _controllerState.UIBeastSwap.ToWallet.ToArray();
            ActionDispatcher.Dispatch(new ShowLoading());
            ActionDispatcher.Dispatch(new SendBeastsToBothSide(inGameBeasts, inDBoxBeasts));
        }
    }
}