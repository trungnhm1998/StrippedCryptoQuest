using UnityEngine;

namespace CryptoQuest.FSM.ScriptableObjects.Base
{
    public abstract class BaseStateSO : ScriptableObject
    {
        public abstract void Execute(BaseStateMachine stateMachine);
        public abstract void OnEnterState(BaseStateMachine stateMachine);
        public abstract void OnExitState(BaseStateMachine stateMachine);
    }
}