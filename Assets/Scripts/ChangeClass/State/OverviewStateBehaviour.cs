using UnityEngine;


namespace CryptoQuest.ChangeClass.StateMachine
{
    public class OverviewStateBehaviour : StateMachineBehaviour
    {
        private ChangeClassStateController _stateController;
        private ChangeClassInputManager _input;
        private Animator _animator;
        private static readonly int _submit = Animator.StringToHash("isChangeClass");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = _animator.GetComponent<ChangeClassStateController>();
            _stateController.Manager.OpenChangeClassEvent += ChangeState;
            _input = _stateController.Input;
            _input.CancelEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _input.CancelEvent -= ExitState;
            _stateController.Manager.OpenChangeClassEvent -= ChangeState;
        }

        private void ChangeState()
        {
            _animator.SetTrigger(_submit);
        }

        private void ExitState()
        {
            _stateController.ExitStateEvent?.Invoke();
        }
    }
}