using UnityEngine;

namespace Indigames.AbilitySystem.FSM
{
    public abstract class BaseStateSO : ScriptableObject
    {
        public virtual void Execute(BaseStateMachine stateMachine) {}
        public virtual void OnEnterState(BaseStateMachine stateMachine) {}
        public virtual void OnExitState(BaseStateMachine stateMachine) {}
    }
}