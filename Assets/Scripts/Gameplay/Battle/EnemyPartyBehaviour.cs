using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyPartyBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EnemyBehaviour> _enemies;

        public void Init(List<EnemyData> loadedEnemyData)
        {
            for (var index = 0; index < loadedEnemyData.Count; index++)
            {
                var enemyData = loadedEnemyData[index];
                if (enemyData == null) continue;
                
                _enemies[index].Init(enemyData);
            }
        }
    }
}