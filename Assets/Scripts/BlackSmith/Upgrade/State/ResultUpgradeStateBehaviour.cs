using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class ResultUpgradeStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isSelect");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController = animator.GetComponent<UpgradeStateController>();
            _animator = animator;
            _stateController.UpgradeResultPanel.SetActive(true);
            _stateController.UpgradeEquipment();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.UpgradeResultPanel.SetActive(false);            
        }

        private void SubmitUpgrade()
        {
            _animator.SetTrigger(_submit);
        }
    }
}