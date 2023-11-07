using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class ConfirmState : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _confirmMessage;

        private Animator _animator;
        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _controller = animator.GetComponent<TavernController>();

            _controller.TavernInputManager.CancelEvent += CancelTransmission;

            _controller.UIGameList.SetInteractableAllButtons(false);
            _controller.UIWalletList.SetInteractableAllButtons(false);

            _controller.DialogsManager.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_confirmMessage)
                .Show();
        }

        private void CancelTransmission()
        {
            _animator.Play(CharacterReplacementState);
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
            _animator.Play(CharacterReplacementState);
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            _animator.Play(CharacterReplacementState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
        }
    }
}