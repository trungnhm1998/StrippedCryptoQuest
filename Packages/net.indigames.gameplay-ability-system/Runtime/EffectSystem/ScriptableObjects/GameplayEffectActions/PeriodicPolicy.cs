using System;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    [Serializable]
    public class PeriodicPolicy : IGameplayEffectPolicy
    {
        /// <summary>
        /// duration of the effect, can be -1 for infinite
        /// </summary>
        [SerializeField] private float _duration = 1f;

        /// <summary>
        /// interval of the effect, can be -1 for infinite
        /// </summary>
        [SerializeField] private float _interval = 1f;
        public PeriodicPolicy() { }
        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) => new(inSpec);
    }
}