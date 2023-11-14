using System;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    /// <summary>
    /// Can be used for counter based effect like turn-based, step counter
    /// </summary>
    [Serializable]
    public class CounterPolicy : IGameplayEffectPolicy
    {
        [SerializeField] protected int _counter;

        public CounterPolicy() { }

        public CounterPolicy(int counter)
        {
            _counter = counter;
        }

        public virtual void RegistCounterEvent(CounterGameplayEffect effect) { }
        /// <summary>
        /// Event should be removed when effect expired or the spec is destroyed
        /// </summary>
        public virtual void RemoveCounterEvent(CounterGameplayEffect effect) { }

        public virtual ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec)
            => new CounterGameplayEffect(this, _counter, inSpec);
    }

    [Serializable]
    public class CounterGameplayEffect : ActiveGameplayEffect
    {
        private event Action ReduceCounter;
        public void ReduceCounterEvent() => ReduceCounter?.Invoke();

        protected float _counter = 0;
        private CounterPolicy _policy;

        public CounterGameplayEffect(CounterPolicy counterPolicy, int counter,
            GameplayEffectSpec effect) : base(effect)
        {
            _counter = counter;
            _policy = counterPolicy;
            _policy.RegistCounterEvent(this);
            ReduceCounter += ReduceStep;
        }

        private void ReduceStep()
        {
            _counter--;
            if (_counter <= 0)
            {
                Spec.IsExpired = true;
                Spec.Target.EffectSystem.RemoveEffect(Spec);
                RemoveRegistEvent();
            }
        }

        private void RemoveRegistEvent()
        {
            ReduceCounter -= ReduceStep;
            _policy.RemoveCounterEvent(this);
        }

        public override void OnRemoved()
        {
            RemoveRegistEvent();
        }
    }
}