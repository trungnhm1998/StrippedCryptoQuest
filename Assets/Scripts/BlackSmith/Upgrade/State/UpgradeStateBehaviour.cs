using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class UpgradeStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private BlackSmithInputManager _input;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isSuccess");
        private static readonly int _exit = Animator.StringToHash("isSelect");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<UpgradeStateController>();
            _stateController.UpgradeEquipmentPanel.SetActive(true);
            _input = _stateController.InputManager;
            _input.SubmitEvent += ChangeState;
            _input.CancelEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.UpgradeEquipmentPanel.SetActive(false);
            _input.SubmitEvent -= ChangeState;
            _input.CancelEvent -= ExitState;
        }

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _animator.SetTrigger(_exit);
        }
    }
}