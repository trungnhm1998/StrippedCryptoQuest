using UnityEngine;

namespace Indigames.AbilitySystem.FSM
{
    public abstract class FSMActionSO : ScriptableObject
    {
        public abstract void Execute(BaseStateMachine stateMachine);
    }
}