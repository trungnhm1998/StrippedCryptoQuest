using System;
using UnityEngine;
using UnityEngine.Animations;

namespace CryptoQuest.Tavern.States.CharacterReplacement
{
    public abstract class StateMachineBehaviourBase : StateMachineBehaviour
    {
        private bool _hasEntered = false;
        protected Animator StateMachine { get; private set; }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _hasEntered = true;
            StateMachine = animator;
            OnEnter();
        }

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller)
        {
            _hasEntered = true;
            StateMachine = animator;
            OnEnter();
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex,
            AnimatorControllerPlayable controller) => OnExit();

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) => OnExit();

        public void Exit()
        {
            if (!_hasEntered) return;
            OnExit();
        }

        protected abstract void OnEnter();
        protected abstract void OnExit();
    }
}