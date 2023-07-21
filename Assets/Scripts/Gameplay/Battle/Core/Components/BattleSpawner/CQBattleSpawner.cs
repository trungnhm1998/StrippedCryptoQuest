using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects.Data;
using CryptoQuest.Gameplay.Battle.Core.Components.BattleUnit;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using System.Collections.Generic;

namespace CryptoQuest.Gameplay.Battle.Core.Components.BattleSpawner
{
    public class CQBattleSpawner : BaseBattleSpawner
    {
        public override void GenerateBattle(BattleDataSO data)
        {
            List<AbilitySystemBehaviour> enemyMembers = _battleManager.BattleTeam2.Members;
            for (int i = 0; i < enemyMembers.Count; i++)
            {
                AbilitySystemBehaviour member = enemyMembers[i];
                var isInEnemyRange = i < data.Enemies.Length;
                member.gameObject.SetActive(isInEnemyRange);
                if (!isInEnemyRange) continue;

                var enemy = data.Enemies[i];
                var statInit = member.GetComponent<StatsInitializer>();
                statInit.InitStats(enemy);

                var battleUnit = member.GetComponent<IBattleUnit>();
                battleUnit.UnitData = enemy;
                ProcessEnemiesName(data, enemy);
            }
        }
    }
}
    