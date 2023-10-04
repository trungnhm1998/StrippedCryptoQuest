using UnityEngine;

namespace CryptoQuest.BlackSmith.EvolveStates
{
    public class ConfirmStateBehaviour : StateMachineBehaviour
    {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Debug.Log("Enter confirm state");

            var stateController = animator.GetComponent<EvolveStateController>();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) { }
    }
}