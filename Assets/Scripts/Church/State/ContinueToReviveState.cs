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
            _stateController.Input.CancelEvent += ExitState;
            SetupDialog();
        }

        protected override void OnExit()
        {
            _stateController.Input.CancelEvent -= ExitState;
        }

        private void ExitState()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(ExitChurch);
            _stateController.IsExitState = true;
        }

        private void YesButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            StateMachine.Play(SelectCharacter);
        }

        private void NoButtonPressed()
        {
            ExitState();
        }

        private void SetupDialog()
        {
            _stateController.DialogController.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_message)
                .Show();
        }
    }
}
