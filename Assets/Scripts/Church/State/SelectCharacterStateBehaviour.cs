using CryptoQuest.Church.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class SelectCharacterStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;
        private static readonly int Confirm = Animator.StringToHash("ConfirmState");
        private static readonly int Overview = Animator.StringToHash("OverviewState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += SelectCharacter;
            _stateController.Input.CancelEvent += ExitState;
            _stateController.Presenter.SelectDefaultButton();
        }

        protected override void OnExit()
        {
            _stateController.Input.SubmitEvent -= SelectCharacter;
            _stateController.Input.CancelEvent -= ExitState;
        }

        private void ChangeState()
        {
            StateMachine.Play(Confirm);
        }

        private void ExitState()
        {
            StateMachine.Play(Overview);
        }

        private void SelectCharacter()
        {
            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();
            _stateController.Presenter.ReviveCharacter(currentCharacter);
            ChangeState();
        }
    }
}
