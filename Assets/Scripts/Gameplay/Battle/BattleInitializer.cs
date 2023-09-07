using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
using CryptoQuest.Gameplay.Character;
using UnityEngine;

namespace CryptoQuest.Gameplay.Battle
{
    public interface IBattleInitializer
    {
        public IEnumerator LoadEnemies();
    }

    public class BattleInitializer : MonoBehaviour, IBattleInitializer
    {
        [SerializeField] private BattleBus _bus;
        [SerializeField] private EnemyPartyBehaviour _enemyPartyBehaviour;
        [SerializeField] private EnemyDatabase _enemyDatabase;

        public IEnumerator LoadEnemies()
        {
            yield return LoadEnemiesData();
            _enemyPartyBehaviour.Init(_loadedEnemyData);
        }

        private readonly List<EnemyData> _loadedEnemyData = new();

        private IEnumerator LoadEnemiesData()
        {
            _loadedEnemyData.Clear();
            var enemyParty = _bus.CurrentBattlefield;
            for (var index = 0; index < enemyParty.EnemyIds.Length; index++)
            {
                var enemyId = enemyParty.EnemyIds[index];
                yield return _enemyDatabase.LoadDataById(enemyId);
                _loadedEnemyData.Add(_enemyDatabase.GetDataById(enemyId)); // TODO: UNLOAD ENEMY DATA
            }
        }
    }
}