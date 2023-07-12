using UnityEngine;

namespace CryptoQuest.FSM
{
    public abstract class DecisionSO : ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine stateMachine);
    }
}