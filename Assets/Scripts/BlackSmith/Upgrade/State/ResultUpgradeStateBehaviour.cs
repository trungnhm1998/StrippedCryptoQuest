using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class ResultUpgradeStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private BlackSmithInputManager _input;
        private UpgradeStateController _stateController;
        private static readonly int _submit = Animator.StringToHash("isSelect");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<UpgradeStateController>();
            _input = _stateController.InputManager;
            _stateController.UpgradeResultPanel.SetActive(true);
            _stateController.UpgradeEquipment();
            _input.SubmitEvent += SubmitUpgrade;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.UpgradeResultPanel.SetActive(false);            
            _input.SubmitEvent -= SubmitUpgrade;
        }

        private void SubmitUpgrade()
        {
            _animator.SetTrigger(_submit);
        }
    }
}