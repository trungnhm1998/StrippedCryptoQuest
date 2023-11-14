using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ConfirmStateBehaviour : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;
        private static readonly int Result = Animator.StringToHash("ResultState");
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.DialogController.Dialogue.SetMessage(_message).Hide();
            _stateController.DialogController.ShowChoiceDialog(_message);
            _stateController.DialogController.YesPressedEvent+= YesButtonPressed;
            _stateController.DialogController.NoPressedEvent+= NoButtonPressed;
        }

        protected override void OnExit()
        {
        }

        private void YesButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            _stateController.Presenter.ReviveCharacter();
            StateMachine.Play(Result);
        }

        private void NoButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(SelectCharacter);
        }
    }
}
