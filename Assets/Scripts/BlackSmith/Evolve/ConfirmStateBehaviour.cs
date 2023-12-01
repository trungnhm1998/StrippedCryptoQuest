using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class ConfirmStateBehaviour : StateMachineBehaviour
    {
        private EvolveStateController _stateController;
        private Animator _animator;
        private MerchantsInputManager _stateControllerInput;
        
        private static readonly int SelectEquipmentState = Animator.StringToHash("isSelectEquipment");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {

            _animator = animator;

            _stateController = animator.GetComponent<EvolveStateController>();
            _stateControllerInput = _stateController.Input;

            _stateController.ConfirmPanel.gameObject.SetActive(true);

            _stateControllerInput.CancelEvent += ExitConfirmState;
        }

        private void ExitConfirmState()
        {
            _animator.SetTrigger(SelectEquipmentState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateControllerInput.CancelEvent -= ExitConfirmState;

            _stateController.ConfirmPanel.gameObject.SetActive(false);
            _stateController.DialogsPresenter.HideConfirmDialog();
        }
    }
}
