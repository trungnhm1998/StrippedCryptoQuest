using System;
using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using UnityEngine;
using UnityEngine.SearchService;

namespace CryptoQuest.Battle
{
    public class EnemySpecialAbilityController : MonoBehaviour
    {
        public static Action<AbilitySystemBehaviour> OnEnemyFled;
        [SerializeField] private EnemyPartyBehaviour _enemyPartyBehaviour;

        private void OnEnable()
        {
            OnEnemyFled += ActivateEnemyFled;
        }

        private void OnDisable()
        {
            OnEnemyFled -= ActivateEnemyFled;
        }

        private void ActivateEnemyFled(AbilitySystemBehaviour abilitySystemBehaviour)
        {
            List<EnemyBehaviour> enemies = _enemyPartyBehaviour.Enemies;
            foreach (var enemy in enemies)
            {
                if (enemy.AbilitySystem != abilitySystemBehaviour) continue;
                enemy.Spec.Release();
            }
        }
    }
}