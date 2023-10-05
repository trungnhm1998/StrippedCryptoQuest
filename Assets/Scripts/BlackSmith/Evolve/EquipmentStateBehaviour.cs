using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EquipmentStateBehaviour : StateMachineBehaviour
    {
        private BlackSmithInputManager _stateControllerInput;
        private Animator _animator;
        private static readonly int ConfirmState = Animator.StringToHash("isConfirm");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;

            Debug.Log("Enter equipment state");
            var stateController = animator.GetComponent<EvolveStateController>();

            _stateControllerInput = stateController.Input;
            _stateControllerInput.CancelEvent += GoToConfirmState;

            stateController.EvolvePanel.SetActive(true);
        }

        private void GoToConfirmState()
        {
            _animator.SetTrigger(ConfirmState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateControllerInput.CancelEvent -= GoToConfirmState;
        }
    }
}