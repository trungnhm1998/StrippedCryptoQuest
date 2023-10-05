using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.UI.CommandDetail;
using CryptoQuest.Character.Enemy;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Battle
{
    public interface IBattleInitializer
    {
        public IEnumerator LoadEnemies();
    }

    public class BattleInitializer : MonoBehaviour, IBattleInitializer
    {
        [SerializeField] private BattleBus _bus;
        [SerializeField] private EnemyPartyBehaviour _enemyPartyBehaviour;
        [SerializeField] private SelectEnemyPresenter _selectEnemyPresenter;
        [SerializeField] private EnemyDatabase _enemyDatabase;
        private readonly List<EnemySpec> _loadedEnemies = new();

        public IEnumerator LoadEnemies()
        {
            yield return LoadEnemiesAssets();
            _enemyPartyBehaviour.Init(_loadedEnemies);
            _selectEnemyPresenter.Init(_enemyPartyBehaviour.Enemies);
        }

        private IEnumerator LoadEnemiesAssets()
        {
            _loadedEnemies.Clear();
            var enemyGroups = _bus.CurrentBattlefield.EnemyGroups;
            for (var index = 0; index < enemyGroups.Length; index++)
            {
                var enemyGroupIds = enemyGroups[index];
                yield return LoadEnemyGroupsAssets(enemyGroupIds.EnemyIds);
            }
        }

        private IEnumerator LoadEnemyGroupsAssets(IList<int> enemyIds)
        {
            var enemyGroup = new EnemyGroup();
            enemyGroup.Init();

            for (var index = 0; index < enemyIds.Count; index++)
            {
                var enemyId = enemyIds[index];
                yield return _enemyDatabase.LoadDataById(enemyId);
                var def = _enemyDatabase.GetDataById(enemyId);
                if (def == null)
                {
                    // TODO: Create mock enemy instead of skipping?
                    Debug.LogError($"failed to load enemy data with id {enemyId}, skipping...");
                    continue;
                }
                enemyGroup.Def = def;
                var spec = def.CreateCharacterSpec();
                enemyGroup.EnemySpecs.Add(spec);
                _loadedEnemies.Add(spec); // TODO: UNLOAD ENEMY DATA
            }
            
            _enemyPartyBehaviour.EnemyGroups.Add(enemyGroup);
        }
    }
}