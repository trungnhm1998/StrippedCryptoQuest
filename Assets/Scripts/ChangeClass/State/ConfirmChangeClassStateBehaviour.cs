using System;
using UnityEngine;
using UnityEngine.Localization;


namespace CryptoQuest.ChangeClass.StateMachine
{
    public class ConfirmChangeClassStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChangeClassStateController _stateController;
        private ChangeClassInputManager _input;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isResult");
        private static readonly int _exit = Animator.StringToHash("isChangeClass");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = _animator.GetComponent<ChangeClassStateController>();
            _input = _stateController.Input;
            _input.CancelEvent += ExitState;
            _stateController.ConfirmMaterial.PreviewData();
            _stateController.DialogController.Dialogue.Hide();
            _stateController.DialogController.ChoiceDialog
                .SetButtonsEvent(YesButtonPressed, NoButtonPressed)
                .SetMessage(_message)
                .Show();
        }
        private void YesButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            ChangeState();
        }

        private void NoButtonPressed()
        {
            _stateController.DialogController.ChoiceDialog.Hide();
            ExitState();
        }


        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.CancelEvent -= ExitState;
        }

        private void ChangeState()
        {
            _stateController.DialogController.Dialogue.Show();
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _animator.SetTrigger(_exit);
            _stateController.Presenter.Init();
        }
    }
}