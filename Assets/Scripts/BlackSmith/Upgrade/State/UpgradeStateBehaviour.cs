using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Upgrade.StateMachine
{
    public class UpgradeStateBehaviour : StateMachineBehaviour
    {
        private InputMediatorSO _stateControllerInput;
        private Animator _animator;
        private static readonly int Property = Animator.StringToHash("isUpgrade");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;
            Debug.Log("Enter equipment state");
            var stateController = animator.GetComponent<UpgradeStateController>();
            _stateControllerInput = stateController.Input;
            _stateControllerInput.MenuInteractEvent += GoToConfirmState;
            stateController.UpgradePanel.SetActive(true);
        }

        private void GoToConfirmState()
        {
            _animator.SetTrigger(Property);
            _animator.Play("Confirm");
        }
    }
}