using UnityEngine;
using IndiGames.GameplayAbilitySystem.AttributeSystem.Components;
using IndiGames.GameplayAbilitySystem.AbilitySystem.Components;

namespace CryptoQuest.Gameplay.Battle
{
    [RequireComponent(typeof(BattleManager))]
    public abstract class BaseBattleSpawner : MonoBehaviour
    {
        [SerializeField] protected BattleManager _battleManager;
        [SerializeField] protected GameObject _monsterPrefab;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_battleManager != null) return;
            _battleManager = GetComponent<BattleManager>();
        }
#endif

        public abstract void SpawnBattle();

        public virtual void GenerateBattle(BattleDataSO data)
        {
            foreach (var enemy in data.Enemies)
            {
                var enemyGO = Instantiate(_monsterPrefab, transform);
                var statInit = enemyGO.GetComponent<StatsInitializer>();
                statInit.InitStats(enemy);
                var abilitySystem = enemyGO.GetComponent<AbilitySystemBehaviour>();
                foreach (var skill in enemy.GrantedSkills)
                {
                    abilitySystem.GiveAbility(skill);
                }
                _battleManager.BattleTeam2.Add(abilitySystem);
            }
        }
    }
}
    