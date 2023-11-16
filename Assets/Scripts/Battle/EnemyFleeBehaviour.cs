using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Battle.Events;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using TinyMessenger;
using UnityEngine;
using UnityEngine.SearchService;

namespace CryptoQuest.Battle
{
    public class EnemyFleeBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemyPartyBehaviour _enemyPartyBehaviour;
        private TinyMessageSubscriptionToken _enemyFledEventToken;

        private void OnEnable()
        {
            _enemyFledEventToken = BattleEventBus.SubscribeEvent<EnemyFledEvent>(ActivateEnemyFled);
        }

        private void OnDisable()
        {
            BattleEventBus.UnsubscribeEvent(_enemyFledEventToken);
        }

        private void ActivateEnemyFled(EnemyFledEvent enemyContext)
        {
            AbilitySystemBehaviour abilitySystemBehaviour = enemyContext.enemySystemBehaviour;
            List<EnemyBehaviour> enemies = _enemyPartyBehaviour.Enemies;
            foreach (var enemy in enemies)
            {
                if (enemy.AbilitySystem != abilitySystemBehaviour) continue;
                enemy.Spec.Release();
            }
        }
    }
}