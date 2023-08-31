using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner
{
    public class CQBattleSpawner : BaseBattleSpawner
    {
        public override void GenerateBattle(ScriptableObjects.Data.EncounterGroups groups)
        {
            // TODO: REFACTOR BATTLE SYSTEM
            // var enemyTeam = _battleManager.BattleTeam2;
            // List<AbilitySystemBehaviour> enemyMembers = enemyTeam.Members;
            // for (int i = 0; i < enemyMembers.Count; i++)
            // {
            //     AbilitySystemBehaviour member = enemyMembers[i];
            //     var isInEnemyRange = i < data.Enemies.Count;
            //     member.gameObject.SetActive(isInEnemyRange);
            //     if (!isInEnemyRange) continue;
            //
            //     var enemy = data.Enemies[i];
            //     var statInit = member.GetComponent<ScriptableObjectStatsInitializer>();
            //     statInit.InitStats(enemy);
            //
            //     var battleUnit = member.GetComponent<BattleUnitBase>();
            //     battleUnit.UnitData = enemy;
            //     battleUnit.CreateCharacterInfo();
            //     
            //     ProcessEnemiesName(data, battleUnit.UnitInfo);
            // }
            // enemyTeam.TeamGroups = new BattleTeamGroups(enemyTeam, data.EnemyGroups);
        }
    }
}
    