using System;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    [Serializable]
    public class DurationalPolicy : IGameplayEffectPolicy
    {
        [SerializeField] private float _duration;
        public DurationalPolicy() { }

        public DurationalPolicy(float duration)
        {
            _duration = duration;
        }

        public ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new DurationGameplayEffect(_duration, inSpec);
    }

    [Serializable]
    public class DurationGameplayEffect : ActiveGameplayEffect
    {
        private float _duration = 0;

        public DurationGameplayEffect(float duration, GameplayEffectSpec effect) : base(effect)
        {
            _duration = duration;
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            _duration -= deltaTime;
            if (_duration <= 0) Spec.IsExpired = true;
        }
    }
}