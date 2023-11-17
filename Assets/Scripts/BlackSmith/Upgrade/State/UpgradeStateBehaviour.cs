using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.State
{
    public class UpgradeStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isSuccess");
        private static readonly int _exit = Animator.StringToHash("isSelect");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<UpgradeStateController>();
            _stateController.UpgradeEquipmentPanel.SetActive(true);
            _stateController.UpgradeEvent += ChangeState;
            _stateController.ExitUpgradeEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.UpgradeEquipmentPanel.SetActive(false);
            _stateController.UpgradeEvent -= ChangeState;
            _stateController.ExitUpgradeEvent -= ExitState;
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