using UnityEngine;

namespace CryptoQuest.System.Cheat.States
{
    public abstract class TerminalStateBase : StateMachineBehaviour
    {
        private CheatManager _manager;
        protected Animator Anim { get; private set;}
        protected CheatManager Manager => _manager;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex)
        {
            Anim = animator;
            _manager ??= animator.GetComponent<CheatManager>();
            Manager.EnableTerminalInput();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateMove(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }

        public override void OnStateIK(Animator animator, AnimatorStateInfo stateInfo,
            int layerIndex) { }
    }
}