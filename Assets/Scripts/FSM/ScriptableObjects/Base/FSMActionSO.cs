using UnityEngine;

namespace CryptoQuest.FSM.ScriptableObjects.Base
{
    public abstract class FSMActionSO : ScriptableObject
    {
        public abstract void Execute(BaseStateMachine stateMachine);
    }
}