using UnityEngine;

namespace Indigames.AbilitySystem.FSM
{
    public abstract class DecisionSO : ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine stateMachine);
    }
}