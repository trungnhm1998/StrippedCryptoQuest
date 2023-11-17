using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern.States
{
    public class OverviewState : StateMachineBehaviourBase
    {
        private TavernController _controller;

        private static readonly int CharacterReplacementState = Animator.StringToHash("Character Replacement Idle");
        private static readonly int PartyOrganizationState = Animator.StringToHash("Party Organization Idle");

        [SerializeField] private LocalizedString _welcomeMsg;

        protected override void OnEnter()
        {
            _controller = StateMachine.GetComponent<TavernController>();
            _controller.TavernUiOverview.gameObject.SetActive(true);

            _controller.TavernUiOverview.CharacterReplacementButtonPressedEvent += EnterCharacterReplacement;
            _controller.TavernUiOverview.PartyOrganizationButtonPressedEvent += EnterPartyOrganization;
            _controller.MerchantInputManager.CancelEvent += ExitTavern;

            ShowDialogue();
        }

        private void ShowDialogue()
        {
            if (_controller.DialogsManager.Dialogue == null) return;
            _controller.DialogsManager.Dialogue
                .SetMessage(_welcomeMsg)
                .SetArrow(false)
                .Show();
        }

        protected override void OnExit()
        {
            _controller.TavernUiOverview.CharacterReplacementButtonPressedEvent -= EnterCharacterReplacement;
            _controller.TavernUiOverview.PartyOrganizationButtonPressedEvent -= EnterPartyOrganization;
            _controller.MerchantInputManager.CancelEvent -= ExitTavern;

            _controller.TavernUiOverview.gameObject.SetActive(false);

            if (_controller.DialogsManager.Dialogue == null) return;
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