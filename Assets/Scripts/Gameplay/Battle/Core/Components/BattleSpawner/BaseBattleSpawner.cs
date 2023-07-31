using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner
{
    [RequireComponent(typeof(BattleManager))]
    public abstract class BaseBattleSpawner : MonoBehaviour
    {
        private readonly string[] _duplicatePostfix = new string[] { "A", "B", "C", "D" };
        [SerializeField] protected BattleManager _battleManager;
        [SerializeField] protected BattleDataSO _battleData;

        private Dictionary<string, int> _duplicateEnemies = new();

        private void OnValidate()
        {
            if (_battleManager != null) return;
            _battleManager = GetComponent<BattleManager>();
        }

        public virtual void SpawnBattle()
        {
            GenerateBattle(_battleData);
        }

        public abstract void GenerateBattle(BattleDataSO data);

        protected void ProcessEnemiesName(BattleDataSO data, CharacterDataSO enemyData)
        {
            var sameNameCount = data.Enemies.Count(x => x.Name == enemyData.Name);
            if (sameNameCount <= 1) return;

            if (!_duplicateEnemies.TryGetValue(enemyData.Name, out var duplicateCount))
            {
                duplicateCount = 0;
                _duplicateEnemies.Add(enemyData.Name, duplicateCount);
            }

            if (duplicateCount >= _duplicatePostfix.Length) return;

            enemyData.DisplayName = $"{enemyData.Name}{_duplicatePostfix[duplicateCount]}";
            _duplicateEnemies[enemyData.Name]++;
        }

        public bool IsBattleEscapale() => _battleData.IsEscapable;
    }
}