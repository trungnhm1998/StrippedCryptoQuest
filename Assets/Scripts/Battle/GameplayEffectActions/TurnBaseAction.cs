using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Battle.GameplayEffectActions
{
    [Serializable]
    public class TurnBaseAction : CounterAction
    {
        public TurnBaseAction() : base() {}
        
        public TurnBaseAction(int counter) : base(counter)
        {
            // TODO: Set next turn event here as reduce counter event
        }
    }
}