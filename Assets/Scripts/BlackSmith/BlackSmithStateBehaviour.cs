using UnityEngine;

namespace CryptoQuest.BlackSmith.StateMachine
{
    public class BlackSmithStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private BlackSmithStateController _stateController;
        private static readonly int _upgrade = Animator.StringToHash("isUpgradeState");
        private static readonly int _evolve = Animator.StringToHash("isEvolveState");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<BlackSmithStateController>();
            _stateController.OpenUpgradeEvent += OpenUpgradeState;
            _stateController.OpenEvolveEvent += OpenEvolveState;
            _stateController.ExitStateEvent?.Invoke(true);
            _stateController.UIBlackSmith.Init();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.OpenUpgradeEvent -= OpenUpgradeState;
            _stateController.OpenEvolveEvent -= OpenEvolveState;
        }

        private void OpenUpgradeState()
        {
            _animator.SetTrigger(_upgrade);
            _stateController.ExitStateEvent?.Invoke(false);
        }

        private void OpenEvolveState()
        {
            _animator.SetTrigger(_evolve);
            _stateController.ExitStateEvent?.Invoke(false);
        }
    }
}
