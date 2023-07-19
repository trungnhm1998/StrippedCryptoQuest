using UnityEngine;

namespace CryptoQuest.FSM.ScriptableObjects.Base
{
    public abstract class DecisionSO : ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine stateMachine);
    }
}