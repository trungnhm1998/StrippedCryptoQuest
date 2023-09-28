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
            AbilitySystemBehaviour owner) => new DurationEffectSpecification(_duration, inSpec);
    }

    [Serializable]
    public class DurationEffectSpecification : ActiveEffectSpecification
    {
        private float _duration = 0;

        public DurationEffectSpecification(float duration, GameplayEffectSpec effect) : base(effect)
        {
            _duration = duration;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _duration -= deltaTime;
            if (_duration <= 0) EffectSpec.IsExpired = true;
        }
    }
}