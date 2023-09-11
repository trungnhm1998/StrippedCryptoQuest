using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Battle.Core.ScriptableObjects;
using CryptoQuest.Gameplay.Battle.ScriptableObjects;
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
            _enemyPartyBehaviour.Init(_loadedEnemies);
        }

        private readonly List<Character.Enemy> _loadedEnemies = new();

        private IEnumerator LoadEnemiesData()
        {
            _loadedEnemies.Clear();
            var enemyParty = _bus.CurrentBattlefield;
            for (var index = 0; index < enemyParty.EnemyIds.Length; index++)
            {
                var enemyId = enemyParty.EnemyIds[index];
                yield return _enemyDatabase.LoadDataById(enemyId);
                var characterSpec = _enemyDatabase
                    .GetDataById(enemyId)
                    .CreateCharacterSpec();
                _loadedEnemies.Add(characterSpec); // TODO: UNLOAD ENEMY DATA
            }
        }
    }
}