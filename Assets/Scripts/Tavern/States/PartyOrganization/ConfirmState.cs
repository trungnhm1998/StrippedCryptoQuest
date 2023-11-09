using CryptoQuest.Tavern.States.CharacterReplacement;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States.PartyOrganization
{
    public class ConfirmState : StateMachineBehaviourBase
    {
        [SerializeField] private LocalizedString _confirmMessage;

        private TavernController _controller;

        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

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
            StateMachine.Play(PartyOrganizationState);
        }

        private void YesButtonPressed()
        {
            _controller.UICharacterReplacement.ConfirmedTransmission();
            StateMachine.Play(PartyOrganizationState);
        }

        private void NoButtonPressed()
        {
            _controller.DialogsManager.ChoiceDialog.Hide();
            StateMachine.Play(PartyOrganizationState);
        }
    }
}