using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Character;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Battle.ScriptableObjects
{
    public class EnemyDatabase : ScriptableObject
    {
        [Serializable]
        public struct Map
        {
            public int Id;
            public AssetReferenceT<EnemyData> Enemy;
        }

        [SerializeField] private Map[] _enemies;

        private Dictionary<int, AssetReferenceT<EnemyData>> _enemyMap = new();
        private Dictionary<int, EnemyData> _loadedEnemyMap = new();

        private void OnEnable()
        {
            _enemyMap.Clear();
            _enemyMap = _enemies.ToDictionary(x => x.Id, x => x.Enemy);
        }

        public IEnumerator AsyncGetEnemyById(int id, Action<EnemyData> callback)
        {
            if (_loadedEnemyMap.TryGetValue(id, out var enemyData))
            {
                callback?.Invoke(enemyData);
                yield break;
            }

            Debug.Log($"EnemyDatabase::GetEnemyById() Attempt to load enemy with id {id}");

            if (_enemyMap.TryGetValue(id, out var enemy))
            {
                var handle = enemy.LoadAssetAsync<EnemyData>();
                yield return handle;
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    _loadedEnemyMap.Add(id, handle.Result);
                    callback?.Invoke(handle.Result);
                    yield break;
                }
            }

            Debug.LogError($"EnemyDatabase::GetEnemyById() - Cannot find/load enemy with id {id}");
            callback?.Invoke(null);
        }
#if UNITY_EDITOR
        public void Editor_SetEnemyMap(Map[] enemies)
        {
            _enemies = enemies;
        }
#endif
    }
}