using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.Components
{
    public abstract class PreEffectExecuteEvent : ScriptableObject
    {
        public abstract bool Execute(GameplayEffectModCallbackData executeData);
    }
}