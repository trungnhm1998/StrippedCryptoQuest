using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class EquipmentStateBehaviour : StateMachineBehaviour
    {
        private BlackSmithInputManager _stateControllerInput;
        private Animator _animator;
        private EvolveStateController _stateController;
        private static readonly int ConfirmState = Animator.StringToHash("isConfirm");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;

            Debug.Log("Enter equipment state");
            _stateController = animator.GetComponent<EvolveStateController>();

            _stateControllerInput = _stateController.Input;
            _stateControllerInput.CancelEvent += GoToConfirmState;

            _stateController.EvolvePanel.SetActive(true);
        }

        private void GoToConfirmState()
        {
            _stateController.EvolvePanel.SetActive(false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateControllerInput.CancelEvent -= GoToConfirmState;
        }
    }
}