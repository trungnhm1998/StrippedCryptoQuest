using FSM;
using System.Collections.Generic;

namespace CryptoQuest.PushdownFSM
{
    public class PushdownStateMachine : StateMachine
    {
        public Stack<StateBase> StateStack { get; } = new Stack<StateBase>();

        public PushdownStateMachine(bool needsExitTime = true) : base(needsExitTime) { }
        
        /// <summary>
        /// Pushdown to previous state if any
        /// If there's only 1 state left, this does nothing
        /// </summary>
        /// <param name="forceInstantly"></param>
        public void PushdownState(bool forceInstantly = false)
        {
            var currentState = StateStack.Pop();
            if (StateStack.Count <= 0)
            {
                StateStack.Push(currentState);
                return;
            } 
            // The state will be push back in at OnEnter
            var previousState = StateStack.Pop();
            base.RequestStateChange(previousState.name, forceInstantly);
        }

        /// <summary>
        /// Pushdown all state and change the root state to new state
        /// </summary>
        /// <param name="stateName"></param>
        /// <param name="forceInstantly"></param>
        public void ResetToNewState(string stateName, bool forceInstantly = false)
        {
            StateStack.Clear();
            base.RequestStateChange(stateName, forceInstantly);
        }
    }
}