using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyPartyBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EnemyBehaviour> _enemies;

        public void Init(List<Character.Enemy> loadedEnemyData)
        {
            for (var index = 0; index < loadedEnemyData.Count; index++)
            {
                var enemySpec = loadedEnemyData[index];
                if (enemySpec == null) continue;
                
                _enemies[index].Init(enemySpec);
            }
        }
    }
}