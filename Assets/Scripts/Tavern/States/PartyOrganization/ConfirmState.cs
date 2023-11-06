using CryptoQuest.Core;
using CryptoQuest.UI.Actions;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class ConfirmState : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _confirmMessage;

        private Animator _animator;
        private TavernController _controller;

        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");

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
            _animator.Play(PartyOrganizationState);
        }

        private void YesButtonPressed()
        {
            _controller.UICharacterReplacement.ConfirmedTransmission();
            _animator.Play(PartyOrganizationState);
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            _animator.Play(PartyOrganizationState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
        }
    }
}