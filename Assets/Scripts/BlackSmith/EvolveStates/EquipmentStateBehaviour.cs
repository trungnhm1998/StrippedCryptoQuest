using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EquipmentStateBehaviour : StateMachineBehaviour
    {
        private InputMediatorSO _stateControllerInput;
        private Animator _animator;
        private static readonly int Property = Animator.StringToHash("New Trigger");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _animator = animator;
            Debug.Log("Enter equipment state");
            var stateController = animator.GetComponent<EvolveStateController>();
            _stateControllerInput = stateController.Input;
            _stateControllerInput.MenuInteractEvent += GoToConfirmState;
        }

        private void GoToConfirmState()
        {
            _animator.SetTrigger(Property);
            _animator.Play("Confirm");
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            _stateControllerInput.MenuInteractEvent -= GoToConfirmState;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }
    }
}