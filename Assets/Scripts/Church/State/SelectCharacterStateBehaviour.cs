using CryptoQuest.Church.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class SelectCharacterStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        [SerializeField] private LocalizedString _overviewMessage;
        private ChurchStateController _stateController;
        private static readonly int Confirm = Animator.StringToHash("ConfirmState");
        private static readonly int ExitReviveState = Animator.StringToHash("ExitState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.Input.SubmitEvent += SelectCharacter;
            _stateController.Input.CancelEvent += ExitState;
            _stateController.Presenter.SelectDefaultButton();
            _stateController.DialogController.Dialogue.SetMessage(_message).Show();
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
            StateMachine.Play(ExitReviveState);
            _stateController.IsExitState = true;
            _stateController.DialogController.Dialogue.Hide();
        }

        private void SelectCharacter()
        {
            var currentCharacter = EventSystem.current.currentSelectedGameObject.GetComponent<UICharacter>();
            _stateController.Presenter.GetCharacter(currentCharacter);
            ChangeState();
        }
    }
}
