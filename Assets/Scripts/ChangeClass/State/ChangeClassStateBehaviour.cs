using UnityEngine;


namespace CryptoQuest.ChangeClass.StateMachine
{
    public class ChangeClassStateBehaviour : StateMachineBehaviour
    {
        private ChangeClassInputManager _input;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isConfirm");
        private static readonly int _exit = Animator.StringToHash("isOverview");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _input.SubmitEvent += ChangeState;
            _input.CancelEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.SubmitEvent -= ChangeState;
            _input.CancelEvent -= ExitState;
        }

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _animator.SetTrigger(_exit);
        }
    }
}