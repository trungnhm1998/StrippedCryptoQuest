using System.Collections.Generic;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Gameplay.Enemy;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public class EnemyPartyBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EnemyBehaviour> _enemies;

        private static readonly string[] Postfixes = { "A", "B", "C", "D" };

        /// <summary>
        /// To separate with other same Enemy in a group
        /// their name will be post fix with A, B, C, D 
        /// </summary>
        public void Init(List<EnemySpec> loadedEnemyData)
        {
            var dict = new Dictionary<EnemyDef, int>();
            for (var index = 0; index < loadedEnemyData.Count; index++)
            {
                var enemySpec = loadedEnemyData[index];
                if (enemySpec == null || enemySpec.IsValid() == false) continue;
                dict.TryAdd(enemySpec.Data, 0);
                StartCoroutine(enemySpec.SetDisplayName(Postfixes[dict[enemySpec.Data]++]));
                _enemies[index].Init(enemySpec);
            }
        }
    }
}