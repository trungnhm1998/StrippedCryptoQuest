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

        public override void GenerateBattle(ScriptableObjects.Data.EncounterGroups groups)
        {
            // TODO: REFACTOR BATTLE SYSTEM
            // foreach (var enemy in data.Enemies)
            // {
            //     GameObject enemyGO = Instantiate(_monsterPrefab, transform);
            //     var statInit = enemyGO.GetComponent<ScriptableObjectStatsInitializer>();
            //     statInit.InitStats(enemy);
            //
            //     var battleUnit = enemyGO.GetComponent<BattleUnitBase>();
            //     battleUnit.UnitData = enemy;
            //     battleUnit.CreateCharacterInfo();
            //     
            //     var abilitySystem = enemyGO.GetComponent<AbilitySystemBehaviour>();
            //     _battleManager.BattleTeam2.Members.Add(abilitySystem);
            //     ProcessEnemiesName(data, battleUnit.UnitInfo);
            // }
        }
    }
}
    