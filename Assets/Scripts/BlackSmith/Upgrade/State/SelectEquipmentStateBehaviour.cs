using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class SelectEquipmentStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private BlackSmithInputManager _input;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isUpgrade");
        private static readonly int _exit = Animator.StringToHash("isBlackSmithState");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<UpgradeStateController>();
            _input = _stateController.InputManager;
            _stateController.SelectedEquipmentPanel.SetActive(true);
            _stateController.SelectActionPanel.SetActive(false);
            _input.SubmitEvent += GoToUpgradeState;
            _input.CancelEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.SelectedEquipmentPanel.SetActive(false);
            _input.SubmitEvent -= GoToUpgradeState;
            _input.CancelEvent -= ExitState;
        }

        private void GoToUpgradeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _stateController.UIBlackSmith.BlackSmithOpened();
            _animator.SetTrigger(_exit);
        }
    }
}