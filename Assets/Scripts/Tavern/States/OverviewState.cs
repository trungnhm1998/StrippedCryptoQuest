using CryptoQuest.Tavern.UI;
using UnityEngine;

namespace CryptoQuest.Tavern.States
{
    public class OverviewState : StateMachineBehaviour
    {
        private Animator _animator;
        private TavernPresenter _presenter;

        private static readonly int CharacterReplacementState = Animator.StringToHash("isCharacterReplacement");
        private static readonly int PartyOrganizationState = Animator.StringToHash("isPartyOrganization");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;

            _presenter = animator.GetComponent<TavernPresenter>();
            _presenter.TavernUiOverview.gameObject.SetActive(true);

            _presenter.TavernUiOverview.CharacterReplacementButtonPressedEvent += EnterCharacterReplacement;
            _presenter.TavernUiOverview.PartyOrganizationButtonPressedEvent += EnterPartyOrganization;

            _presenter.TavernInputManager.CancelEvent += ExitTavern;
        }

        private void ExitTavern()
        {
            _presenter.TavernUiOverview.gameObject.SetActive(false);
            _presenter.ExitTavernEvent?.Invoke();
        }

        private void EnterCharacterReplacement()
        {
            _animator.SetTrigger(CharacterReplacementState);
        }

        private void EnterPartyOrganization()
        {
            _animator.SetTrigger(PartyOrganizationState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _presenter.TavernUiOverview.CharacterReplacementButtonPressedEvent -= EnterCharacterReplacement;
            _presenter.TavernUiOverview.PartyOrganizationButtonPressedEvent -= EnterPartyOrganization;

            _presenter.TavernInputManager.CancelEvent -= ExitTavern;
        }
    }
}