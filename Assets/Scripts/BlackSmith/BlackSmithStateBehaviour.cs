using System.Collections;
using System.Collections.Generic;
using CryptoQuest.BlackSmith.EvolveStates;
using CryptoQuest.BlackSmith.Upgrade.StateMachine;
using UnityEngine;

namespace CryptoQuest.BlackSmith.StateMachine
{
    public class BlackSmithStateBehaviour : StateMachineBehaviour
    {
        private Animator _animator;
        private BlackSmithInputManager _input;
        private BlackSmithStateController _stateController;
        private static readonly int _upgrade = Animator.StringToHash("isUpgradeState");
        private static readonly int _evolve = Animator.StringToHash("isEvolveState");

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _animator = animator;
            _stateController = animator.GetComponent<BlackSmithStateController>();
            _stateController.OpenUpgradeEvent += OpenUpgradeState;
            _stateController.OpenEvolveEvent += OpenEvolveState;
            _input = _stateController.Input;
            _input.CancelEvent += ExitState;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _stateController.OpenUpgradeEvent -= OpenUpgradeState;
            _stateController.OpenEvolveEvent -= OpenEvolveState;
            _input.CancelEvent -= ExitState;
        }

        private void OpenUpgradeState()
        {
            _animator.SetTrigger(_upgrade);
        }

        private void OpenEvolveState()
        {
            _animator.SetTrigger(_evolve);
        }

        private void ExitState()
        {
            _stateController.CloseStateEvent?.Invoke();
        }
    }
}
