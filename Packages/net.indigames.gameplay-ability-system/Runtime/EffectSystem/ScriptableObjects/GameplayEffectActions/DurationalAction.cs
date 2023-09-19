using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    [Serializable]
    public class DurationalAction : IGameplayEffectAction
    {
        [SerializeField] private float _duration;
        public DurationalAction() {}
        public DurationalAction(float duration)
        {
            _duration = duration;
        }

        public ActiveEffectSpecification CreateActiveEffect(
            GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner) => new(inSpec);
    }
}