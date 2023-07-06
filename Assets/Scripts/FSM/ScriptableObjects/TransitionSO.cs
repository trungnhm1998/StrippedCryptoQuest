using UnityEngine;

namespace CryptoQuest.FSM
{
    [CreateAssetMenu(fileName = "TransitionSO", menuName = "Gameplay/Battle/FSM/Transition")]
    public class TransitionSO : ScriptableObject
    {
        public DecisionSO Decision;
        public BaseStateSO TrueState;
        public BaseStateSO FalseState;

        public virtual void Execute(BaseStateMachine stateMachine)
        {
            var nextState = Decision.Decide(stateMachine) ? TrueState : FalseState;
            if (nextState == null) return;

            stateMachine.SetCurrentState(nextState);
        }
    }
}