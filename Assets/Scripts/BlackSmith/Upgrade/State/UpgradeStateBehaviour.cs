using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class UpgradeStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isSuccess");
        private static readonly int _exit = Animator.StringToHash("isSelect");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController = animator.GetComponent<UpgradeStateController>();
            _animator = animator;
            _stateController.UpgradeEquipmentPanel.SetActive(true);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.UpgradeEquipmentPanel.SetActive(false);
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