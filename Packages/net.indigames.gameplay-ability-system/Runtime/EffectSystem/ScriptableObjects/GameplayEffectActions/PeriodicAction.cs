using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    [Serializable]
    public class PeriodicAction : IGameplayEffectAction
    {
        /// <summary>
        /// duration of the effect, can be -1 for infinite
        /// </summary>
        [SerializeField] private float _duration = 1f;
        
        /// <summary>
        /// interval of the effect, can be -1 for infinite
        /// </summary>
        [SerializeField] private float _interval = 1f;
        public PeriodicAction() { }
        public ActiveEffectSpecification CreateActiveEffect(
            GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner) => new(inSpec);
    }
}