using System;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;

namespace CryptoQuest.Gameplay.Encounter.GameplayEffectActions
{
    [Serializable]
    public class StepCounterAction : CounterAction
    {
        [SerializeField] private GameplayBus _gameplayBus;

        public StepCounterAction() : base() {}
        
        public StepCounterAction(int counter) : base(counter)
        {
            _gameplayBus.Hero.Step += CounterEffectSpecification.ReduceCounterEvent;
        }

        public override void RemoveCounterEvent()
        {
            base.RemoveCounterEvent();
            _gameplayBus.Hero.Step -= CounterEffectSpecification.ReduceCounterEvent;
        }
    }
}