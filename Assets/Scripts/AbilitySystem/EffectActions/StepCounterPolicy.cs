using System;
using CryptoQuest.Character.Behaviours;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.EffectActions
{
    [Serializable]
    public class StepCounterPolicy : CounterPolicy
    {
        [SerializeField] private GameplayBus _gameplayBus;

        public StepCounterPolicy() : base() {}

        public override void RegistCounterEvent(CounterGameplayEffect effect)
        {
            base.RegistCounterEvent(effect);
            if (_gameplayBus.Hero.TryGetComponent(out StepBehaviour stepBehaviourComponent))
                stepBehaviourComponent.Step += effect.ReduceCounterEvent;
        }

        public override void RemoveCounterEvent(CounterGameplayEffect effect)
        {
            base.RemoveCounterEvent(effect);
            if (_gameplayBus.Hero.TryGetComponent(out StepBehaviour stepBehaviourComponent))
                stepBehaviourComponent.Step -= effect.ReduceCounterEvent;
        }

        public override ActiveGameplayEffect CreateActiveEffect(GameplayEffectSpec inSpec) =>
            new StepCounterGameplayEffect(this, _counter, inSpec);
    }

    [Serializable]
    public class StepCounterGameplayEffect : CounterGameplayEffect 
    {
        // Define in master data 
        // https://docs.google.com/spreadsheets/d/1WkX1DyDOGf6EiAppo8Buz2sUkSKV5OnDENEvmHzKXNQ/edit#gid=1108725858
        private const int MAX_STEPS = 99; 

        public StepCounterGameplayEffect(CounterPolicy counterPolicy, int counter,
            GameplayEffectSpec effect) : base(counterPolicy, counter, effect)
        {
            var context = GameplayEffectContext.ExtractEffectContext(effect.Context);
            _counter = (context == null || context.Turns == 0) ? _counter : GetContextStep(context);
        }

        private float GetContextStep(GameplayEffectContext context)
        {
            return (context.Turns != MAX_STEPS) ? context.Turns : Mathf.Infinity;
        }
    }
}