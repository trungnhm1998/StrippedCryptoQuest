using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.CustomApplicationRequirement
{
    public abstract class EffectCustomApplicationRequirement : ScriptableObject
    {
        public abstract bool CanApplyEffect(EffectScriptableObject effect, GameplayEffectSpec effectSpecSpec,
            AbilitySystemBehaviour ownerSystem);
    }
}