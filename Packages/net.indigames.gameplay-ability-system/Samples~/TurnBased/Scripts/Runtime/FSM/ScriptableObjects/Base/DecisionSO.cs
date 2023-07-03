using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.FSM
{
    public abstract class DecisionSO : ScriptableObject
    {
        public abstract bool Decide(BaseStateMachine stateMachine);
    }
}