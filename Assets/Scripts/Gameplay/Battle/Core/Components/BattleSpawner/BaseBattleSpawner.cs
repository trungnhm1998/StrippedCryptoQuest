using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;
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
                enemyGO.name = enemy.Name;
                var abilitySystem = enemyGO.GetComponent<AbilitySystemBehaviour>();
                foreach (var skill in enemy.GrantedSkills)
                {
                    abilitySystem.GiveAbility(skill);
                }
                _battleManager.BattleTeam2.Members.Add(abilitySystem);
                ProcessEnemyName(enemy.Name);
            }
        }

        private void ProcessEnemyName(string enemyName)
        {
            List<AbilitySystemBehaviour> sameNameEnemies = _battleManager.BattleTeam2.Members.FindAll(x => x.gameObject.name == enemyName);
            for (int i = 0; i < sameNameEnemies.Count; i++)
            {
                if (i >= _duplicatePostfix.Length)
                {
                    Debug.LogWarning($"Only allow {_duplicatePostfix.Length} enemy with same name");
                    break;
                }
                sameNameEnemies[i].gameObject.name += _duplicatePostfix[i]; 
            }
        }
    }
}
    