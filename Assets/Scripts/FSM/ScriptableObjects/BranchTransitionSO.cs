using UnityEngine;

namespace CryptoQuest.FSM
{
    [CreateAssetMenu(fileName = "BranchTransitionSO", menuName = "Gameplay/Battle/FSM/Branch Transition")]
    public class BranchTransitionSO : TransitionSO
    {
        public BaseStateSO FalseState;

        public override void Execute(BaseStateMachine stateMachine)
        {
            BaseStateSO nextState = Decision.Decide(stateMachine) ? TrueState : FalseState;

            stateMachine.SetCurrentState(nextState);
        }
    }
}