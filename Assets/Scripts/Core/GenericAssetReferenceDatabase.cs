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

        public event Action<TSerializableObject> DataLoaded;

        [field: SerializeField] public Map[] Maps { get; private set; }

        [NonSerialized] private Dictionary<TKey, AssetReferenceT<TSerializableObject>> _map = new();

        public Dictionary<TKey, AssetReferenceT<TSerializableObject>> CacheMap
        {
            get
            {
                if (_map.Count == 0)
                    _map = Maps.ToDictionary(x => x.Id, x => x.Data);
                return _map;
            }
        }

        private readonly Dictionary<TKey, AsyncOperationHandle<TSerializableObject>> _loadedData = new();

        public IEnumerator LoadDataById(TKey id)
        {
            if (_loadedData.TryGetValue(id, out var loadingHandle))
            {
                yield return loadingHandle;
                yield break;
            }

            Debug.Log($"Loading {id}");
            if (!CacheMap.TryGetValue(id, out var assetRef))
            {
                Debug.LogWarning($"Cannot find asset with id {id} in database");
                yield break;
            }

            var handle = assetRef.LoadAssetAsync();
            _loadedData.TryAdd(id, handle); // means we loading it
            yield return handle;
            if (handle.Status != AsyncOperationStatus.Succeeded || handle.Result == null)
            {
                Debug.LogWarning($"Failed to load asset {assetRef} at id {id}");
                yield break;
            }

            _loadedData[id] = handle;
            DataLoaded?.Invoke(handle.Result);
        }

        /// <summary>
        /// Return the handle can use this for loading progress or check if the data is loaded
        /// Unloading, etc
        ///
        /// Use <see cref="AsyncOperationHandle{TObject}.IsValid"/> to check if the handle is valid
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public AsyncOperationHandle<TSerializableObject> GetHandle(TKey id)
            => _loadedData.TryGetValue(id, out var handle)
                ? handle
                : new AsyncOperationHandle<TSerializableObject>();

        public TSerializableObject GetDataById(TKey id)
        {
            if (_loadedData.TryGetValue(id, out var data))
                return data.Result;

            Debug.LogWarning($"Database::GetDataById() - Cannot find/load data with id {id}");
            return null; // try not to return null
        }
#if UNITY_EDITOR
        public void Editor_SetMaps(Map[] maps)
        {
            Maps = maps;
        }
#endif
    }
}