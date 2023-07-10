using UnityEngine;

namespace CryptoQuest.FSM
{
    public abstract class FSMActionSO : ScriptableObject
    {
        public abstract void Execute(BaseStateMachine stateMachine);
    }
}