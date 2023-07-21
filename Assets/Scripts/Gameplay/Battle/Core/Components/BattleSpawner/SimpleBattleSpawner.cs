using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner
{
    public class SimpleBattleSpawner : BaseBattleSpawner
    {
        [SerializeField] protected GameObject _monsterPrefab;

        public override void GenerateBattle(BattleDataSO data)
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
    }
}
    