using UnityEngine;


namespace CryptoQuest.ChangeClass.StateMachine
{
    public class ResultStateBehaviour : StateMachineBehaviour
    {
        private ChangeClassStateController _stateController;
        private ChangeClassInputManager _input;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isChangeClass");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = _animator.GetComponent<ChangeClassStateController>();
            _input = _stateController.Input;
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