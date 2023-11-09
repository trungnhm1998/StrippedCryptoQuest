using CryptoQuest.Tavern.States.CharacterReplacement;
using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class OverviewState : StateMachineBehaviourBase
    {
        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");
        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.TavernUiOverview.gameObject.SetActive(true);

            _controller.TavernUiOverview.CharacterReplacementButtonPressedEvent += EnterCharacterReplacement;
            _controller.TavernUiOverview.PartyOrganizationButtonPressedEvent += EnterPartyOrganization;

            _controller.TavernInputManager.CancelEvent += ExitTavern;
            _controller.DialogsManager.EnableOverviewButtonsEvent += EnableOverviewButtonsRequested;
        }

        private void EnableOverviewButtonsRequested()
        {
            _controller.TavernUiOverview.EnableOverviewButtons();
        }

        protected override void OnExit()
        {
            _controller.TavernUiOverview.CharacterReplacementButtonPressedEvent -= EnterCharacterReplacement;
            _controller.TavernUiOverview.PartyOrganizationButtonPressedEvent -= EnterPartyOrganization;

            _controller.TavernInputManager.CancelEvent -= ExitTavern;
            _controller.DialogsManager.EnableOverviewButtonsEvent -= EnableOverviewButtonsRequested;

            _controller.TavernUiOverview.gameObject.SetActive(false);
            _controller.DialogsManager.Dialogue.Hide();
        }

        private void ExitTavern()
        {
            _controller.DialogsManager.TavernExited();
            _controller.TavernUiOverview.gameObject.SetActive(false);
            _controller.ExitTavernEvent?.Invoke();
        }

        private void EnterCharacterReplacement()
        {
            StateMachine.Play(CharacterReplacementState);
        }

        private void EnterPartyOrganization()
        {
            StateMachine.Play(PartyOrganizationState);
        }
    }
}