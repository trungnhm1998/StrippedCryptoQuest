using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    public abstract class GameplayEffectActionBase : ScriptableObject
    {
        public abstract ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner);
    }
}