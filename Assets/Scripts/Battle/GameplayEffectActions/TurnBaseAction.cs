using System;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.EffectSystem;
using IndiGames.GameplayAbilitySystem.EffectSystem.ScriptableObjects.GameplayEffectActions;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Battle.GameplayEffectActions
{
    [Serializable]
    public class TurnBaseAction : IGameplayEffectAction
    {
        [SerializeField] private int _turnDuration;
        public TurnBaseAction() { }

        public ActiveEffectSpecification CreateActiveEffect(GameplayEffectSpec inSpec, AbilitySystemBehaviour owner)
        {
            throw new NotImplementedException();
        }
    }
}