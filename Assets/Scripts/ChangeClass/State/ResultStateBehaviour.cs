using UnityEngine;


namespace CryptoQuest.ChangeClass.StateMachine
{
    public class ResultStateBehaviour : StateMachineBehaviour
    {
        private ChangeClassInputManager _input;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isChangeClass");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _input.SubmitEvent += ChangeState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.SubmitEvent -= ChangeState;
        }

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
        }
    }
}