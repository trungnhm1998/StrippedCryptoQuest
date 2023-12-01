using UnityEngine;

namespace CryptoQuest.BlackSmith.Evolve
{
    public class WaitForAvailableDataStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private EvolveStateController _stateController;
        private static readonly int SelectEquipmentState = Animator.StringToHash("isSelectEquipment");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;

            _stateController = animator.GetComponent<EvolveStateController>();
            _stateController.EvolvePanel.gameObject.SetActive(true);
        }

        private void DataAvailable()
        {
            _animator.SetTrigger(SelectEquipmentState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
        }
    }
}