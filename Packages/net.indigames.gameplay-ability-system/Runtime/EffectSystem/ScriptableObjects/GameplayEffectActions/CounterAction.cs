using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;

namespace IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions
{
    /// <summary>
    /// Can be used for counter based effect like turn-based, step counter
    /// </summary>
    [Serializable]
    public class CounterAction : IGameplayEffectAction
    {
        [SerializeField] protected int _counter;

        public CounterAction() {}
        
        public CounterAction(int counter)
        {
            _counter = counter;
        }

        /// <summary>
        /// Event should be removed when effect expired or the spec is destroyed
        /// </summary>
        public virtual void RemoveCounterEvent() { }

        public ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec,
            AbilitySystemBehaviour owner)
                => new CounterEffectSpecification(this, _counter, inSpec);
    }

    [Serializable]
    public class CounterEffectSpecification : ActiveEffectSpecification
    {
        public static event Action ReduceCounter;
        public static void ReduceCounterEvent() => ReduceCounter?.Invoke();

        private int _counter = 0;
        private CounterAction _action;

        public CounterEffectSpecification(CounterAction counterAction, int counter,
            GameplayEffectSpec effect) : base(effect)
        {
            _counter = counter;
            _action = counterAction;
            ReduceCounter += ReduceStep;
        }

        private void ReduceStep()
        {
            _counter--;
            if (_counter <= 0) 
            {
                EffectSpec.IsExpired = true;
                RemoveRegistEvent();
            }
        }

        private void RemoveRegistEvent()
        {
            ReduceCounter -= ReduceStep;
            _action.RemoveCounterEvent();
        }

        ~CounterEffectSpecification()
        {
            RemoveRegistEvent();
        }
    }
}