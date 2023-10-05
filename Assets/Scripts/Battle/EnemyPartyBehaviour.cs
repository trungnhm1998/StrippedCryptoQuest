using System.Collections.Generic;
using CryptoQuest.Battle.Components;
using CryptoQuest.Character.Enemy;
using UnityEngine;

namespace CryptoQuest.Battle
{
    public class EnemyPartyBehaviour : MonoBehaviour
    {
        [SerializeField] private List<EnemyBehaviour> _enemies;
        public List<EnemyBehaviour> Enemies => _enemies;

        public List<EnemyGroup> EnemyGroups { get; private set; } = new();

        private static readonly string[] Postfixes = { "A", "B", "C", "D" };

        private IGameObjectAlign<EnemyBehaviour> _enemyAlign = new EnemiesCenterAlign();

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
                _enemies[index].Init(enemySpec, Postfixes[dict[enemySpec.Data]++]);
                AddEnemyToGroup(_enemies[index], enemySpec);
            }

            _enemyAlign?.Align(_enemies);
        }

        private void AddEnemyToGroup(EnemyBehaviour enemy, EnemySpec spec)
        {
            var group = EnemyGroups.Find(g => g.EnemySpecs.Contains(spec));
            if (!group.IsValid()) return;
            group.Enemies.Add(enemy);
        }
    }
}