using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public class ConfirmState : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _confirmMessage;

        private Animator _animator;
        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Select Character");

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