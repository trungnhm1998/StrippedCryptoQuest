using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class ConfirmState : StateMachineBehaviourBase
    {
        [SerializeField] private LocalizedString _confirmMessage;

        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");
        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();

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
            StateMachine.Play(CharacterReplacementState);
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            StateMachine.Play(CharacterReplacementState);
        }
    }
}