using System.Collections.Generic;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Input;
using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
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
            _stateController.EvolvePanel.WaitForDataAvailableEvent += DataAvailable;
        }

        private void DataAvailable()
        {
            _animator.SetTrigger(SelectEquipmentState);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.EvolvePanel.WaitForDataAvailableEvent -= DataAvailable;
        }
    }
}