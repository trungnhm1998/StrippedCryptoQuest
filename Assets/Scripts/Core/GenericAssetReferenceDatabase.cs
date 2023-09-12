using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace CryptoQuest.Core
{
    public class GenericAssetReferenceDatabase<TKey, TSerializableObject> : ScriptableObject
        where TSerializableObject : Object
    {
        [Serializable]
        public struct Map
        {
            public TKey Id;
            public AssetReferenceT<TSerializableObject> Data;
        }

        [field: SerializeField] public Map[] Maps { get; private set; }

        private Dictionary<TKey, AssetReferenceT<TSerializableObject>> _map = new();

        public Dictionary<TKey, AssetReferenceT<TSerializableObject>> CacheMap
        {
            get
            {
                if (_map.Count == 0)
                    _map = Maps.ToDictionary(x => x.Id, x => x.Data);
                return _map;
            }
        }

        private readonly Dictionary<TKey, TSerializableObject> _loadedData = new();

        public IEnumerator LoadDataById(TKey id)
        {
            if (_loadedData.ContainsKey(id))
                yield break;
            if (!CacheMap.TryGetValue(id, out var assetRef))
            {
                Debug.LogWarning($"Cannot find asset with id {id} in database");
                yield break;
            }

            var handle = assetRef.LoadAssetAsync();
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
            {
                Debug.LogWarning($"Failed to load asset {assetRef} at id {id}");
                yield break;
            }

            _loadedData.Add(id, handle.Result);
        }

        public TSerializableObject GetDataById(TKey id)
        {
            if (_loadedData.TryGetValue(id, out var data))
                return data;

            Debug.LogWarning($"Database::GetDataById() - Cannot find/load data with id {id}");
            return null; // try not to return null
        }
    }
}