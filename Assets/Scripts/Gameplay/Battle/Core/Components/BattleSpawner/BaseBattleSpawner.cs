using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.ScriptableObjects;
using System.Linq;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle
{
    [RequireComponent(typeof(BattleManager))]
    public abstract class BaseBattleSpawner : MonoBehaviour
    {
        private readonly string[] _duplicatePostfix = new string[] {"A", "B", "C", "D"};
        [SerializeField] protected BattleManager _battleManager;
        [SerializeField] protected GameObject _monsterPrefab;

        private Dictionary<string, int> _duplicateEnemies = new();

        private void OnValidate()
        {
            if (_battleManager != null) return;
            _battleManager = GetComponent<BattleManager>();
        }

        public abstract void SpawnBattle();

        public virtual void GenerateBattle(BattleDataSO data)
        {
            foreach (var enemy in data.Enemies)
            {
                GameObject enemyGO = Instantiate(_monsterPrefab, transform);
                var statInit = enemyGO.GetComponent<StatsInitializer>();
                statInit.InitStats(enemy);

                var battleUnit = enemyGO.GetComponent<IBattleUnit>();
                battleUnit.UnitData = enemy;
                
                var abilitySystem = enemyGO.GetComponent<AbilitySystemBehaviour>();
                _battleManager.BattleTeam2.Members.Add(abilitySystem);
                ProcessEnemiesName(data, enemy);
            }
        }

        private void ProcessEnemiesName(BattleDataSO data, CharacterDataSO enemyData)
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
    }
}
    