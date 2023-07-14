using UnityEngine;

namespace CryptoQuest.FSM
{
    [CreateAssetMenu(fileName = "TransitionSO", menuName = "Gameplay/Battle/FSM/Transition")]
    public class TransitionSO : ScriptableObject
    {
        public DecisionSO Decision;
        public BaseStateSO TrueState;

        public virtual void Execute(BaseStateMachine stateMachine)
        {
            if (!Decision.Decide(stateMachine)) return;

            stateMachine.SetCurrentState(TrueState);
        }
    }
}