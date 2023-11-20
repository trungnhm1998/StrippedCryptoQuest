using System;
using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
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

            _controllerState.UIBeastSwap.InGameBeastList.SetEnableButtons(false);
            _controllerState.UIBeastSwap.WalletBeastList.SetEnableButtons(false);

            _controllerState.Controller.DialogManager.ChoiceDialog
                .SetButtonsEvent(OnConfirm, OnCancel)
                .SetMessage(_confirmMessage)
                .Show();
        }

        private void CancelTransmission()
        {
            StateMachine.Play(SwapState);
        }

        protected override void OnExit()
        {
            _controllerState.Controller.Input.SubmitEvent -= BackToSwapState;

            ActionDispatcher.Unbind(_transferSucceedEvent);
            ActionDispatcher.Unbind(_transferFailedEvent);

            _controllerState.Controller.DialogManager.ChoiceDialog.Hide();
        }

        private void OnConfirm()
        {
            ProceedToSendBeasts();
            _controllerState.UIBeastSwap.ConfirmedTransmission();
        }

        private void OnCancel()
        {
            _controllerState.Controller.DialogManager.ChoiceDialog.Hide();
            StateMachine.Play(SwapState);
        }


        private void ShowTransferFailedMessage(ActionBase _)
        {
            HandleTransferMessage(_transferSucceededMsg);
        }

        private void ShowTransferSucceededMessage(ActionBase _)
        {
            Debug.LogError($"ShowTransferSucceededMessage::{_}");
            ActionDispatcher.Dispatch(new ShowLoading(false));
            HandleTransferMessage(_transferFailedMsg);
        }

        private void HandleTransferMessage(LocalizedString msg)
        {
            _controllerState.Controller.DialogManager.NormalDialogue
                .SetMessage(msg)
                .Show();

            _controllerState.Controller.Input.SubmitEvent += BackToSwapState;
        }

        private void BackToSwapState()
        {
            _controllerState.Controller.DialogManager.NormalDialogue.Hide();
            StateMachine.Play(SwapState);
        }

        private void ProceedToSendBeasts()
        {
            List<int> listBeastInGameToTransfer = _controllerState.UIBeastSwap.SelectedInGameBeatIds;
            List<int> listBeastWalletToTransfer = _controllerState.UIBeastSwap.SelectedWalletBeatIds;

            ActionDispatcher.Dispatch(new ShowLoading());

            switch (listBeastInGameToTransfer.Count)
            {
                case > 0 when listBeastWalletToTransfer.Count > 0:
                    ActionDispatcher.Dispatch(new SendBeastsToBothSide(listBeastInGameToTransfer.ToArray(),
                        listBeastWalletToTransfer.ToArray()));
                    break;
                case > 0:
                    ActionDispatcher.Dispatch(new SendBeastsToWallet(listBeastInGameToTransfer.ToArray()));
                    break;
                case <= 0 when listBeastWalletToTransfer.Count > 0:
                    ActionDispatcher.Dispatch(new SendBeastsToGame(listBeastWalletToTransfer.ToArray()));
                    break;
            }
        }
    }
}