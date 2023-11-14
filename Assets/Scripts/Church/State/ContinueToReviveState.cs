using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Church.State
{
    public class ContinueToReviveState : BaseStateBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChurchStateController _stateController;
        private static readonly int ExitChurch = Animator.StringToHash("ExitState");
        private static readonly int SelectCharacter = Animator.StringToHash("SelectCharacterState");

        protected override void OnEnter()
        {
            _stateController = StateMachine.GetComponent<ChurchStateController>();
            _stateController.DialogController.Dialogue.SetMessage(_message).Hide();
            _stateController.DialogController.ShowChoiceDialog(_message);
            _stateController.DialogController.YesPressedEvent += YesButtonPressed;
            _stateController.DialogController.NoPressedEvent += NoButtonPressed;
            _stateController.IsExitState = false;
        }

        protected override void OnExit() { }

        private void YesButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(SelectCharacter);
        }

        private void NoButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            _stateController.IsExitState = true;
            StateMachine.Play(ExitChurch);
        }
    }
}
