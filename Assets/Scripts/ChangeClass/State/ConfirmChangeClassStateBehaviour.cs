using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.ChangeClass.State
{
    public class ConfirmChangeClassStateBehaviour : StateMachineBehaviour
    {
        [SerializeField] private LocalizedString _message;
        private ChangeClassStateController _stateController;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isResult");
        private static readonly int _exit = Animator.StringToHash("isChangeClass");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = _animator.GetComponent<ChangeClassStateController>();
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

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _animator.SetTrigger(_exit);
            _stateController.Presenter.Init();
        }
    }
}