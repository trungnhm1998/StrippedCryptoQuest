using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class OverviewState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");
        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _controller = animator.GetComponent<TavernController>();
            _controller.TavernUiOverview.gameObject.SetActive(true);

            _controller.TavernUiOverview.CharacterReplacementButtonPressedEvent += EnterCharacterReplacement;
            _controller.TavernUiOverview.PartyOrganizationButtonPressedEvent += EnterPartyOrganization;

            _controller.TavernInputManager.CancelEvent += ExitTavern;
        }

        private void ExitTavern()
        {
            _controller.TavernUiOverview.gameObject.SetActive(false);
            _controller.ExitTavernEvent?.Invoke();
        }

        private void EnterCharacterReplacement()
        {
            _animator.Play(CharacterReplacementState);
        }

        private void EnterPartyOrganization()
        {
            _animator.Play(PartyOrganizationState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _controller.TavernUiOverview.CharacterReplacementButtonPressedEvent -= EnterCharacterReplacement;
            _controller.TavernUiOverview.PartyOrganizationButtonPressedEvent -= EnterPartyOrganization;

            _controller.TavernInputManager.CancelEvent -= ExitTavern;

            _controller.TavernUiOverview.gameObject.SetActive(false);
            _controller.DialogsManager.HideDialogue();
        }
    }
}