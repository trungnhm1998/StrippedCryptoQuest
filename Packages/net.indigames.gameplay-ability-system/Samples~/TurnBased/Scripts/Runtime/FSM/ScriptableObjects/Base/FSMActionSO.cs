using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.FSM
{
    public abstract class FSMActionSO : ScriptableObject
    {
        public abstract void Execute(BaseStateMachine stateMachine);
    }
}