using System;
using CryptoQuest.Gameplay;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;

namespace CryptoQuest.AbilitySystem.EffectActions
{
    [Serializable]
    public class StepCounterPolicy : CounterPolicy
    {
        [SerializeField] private GameplayBus _gameplayBus;

        public StepCounterPolicy() : base() {}
        
        public StepCounterPolicy(int counter) : base(counter)
        {
            _gameplayBus.Hero.Step += CounterGameplayEffect.ReduceCounterEvent;
        }

        public override void RemoveCounterEvent()
        {
            base.RemoveCounterEvent();
            _gameplayBus.Hero.Step -= CounterGameplayEffect.ReduceCounterEvent;
        }
    }
}