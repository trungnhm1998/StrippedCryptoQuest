using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace IndiGames.Core.Database
{
    public abstract class AssetReferenceDatabaseT : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeReference] private List<IPlugin> _plugins;
        public abstract Type GetAssetType();
        public abstract void Editor_FetchDataInProject();
#endif
    }

    public class AssetReferenceDatabaseT<TKey, TSerializableObject> : AssetReferenceDatabaseT
        where TSerializableObject : Object
    {
        [Serializable]
        public struct Map
        {
            public TKey Id;
            public AssetReferenceT<TSerializableObject> Data;
        }

        public event Action<TSerializableObject> DataLoaded;
        [field: SerializeField] private Map[] _maps;
        public Map[] Maps => _maps;

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
                if (loadingHandle.IsValid())
                {
                    yield return loadingHandle;
                    yield break;
                }

                _loadedData.Remove(id);
            }

            Debug.Log($"Loading {name}: {id}");
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
            return default(TSerializableObject);
        }

        public bool TryGetDataById(TKey id, out TSerializableObject asset)
        {
            asset = default(TSerializableObject);
            if (_loadedData.TryGetValue(id, out var data))
            {
                asset = data.Result;
                return true;
            }

            Debug.LogWarning($"Database::GetDataById() - Cannot find/load data with id {id}");
            return false;
        }

#if UNITY_EDITOR
        public void Editor_SetMaps(Map[] maps)
        {
            _maps = maps;
        }

        /// <summary>
        /// You must also declare this in your devired class so it'll show up in editor inspector menu
        /// </summary>
        public override void Editor_FetchDataInProject()
        {
            _maps = Array.Empty<Map>();

            var assetUids = AssetDatabase.FindAssets("t:" + typeof(TSerializableObject).Name);

            foreach (var uid in assetUids)
            {
                var instance = new Map();
                var path = AssetDatabase.GUIDToAssetPath(uid);
                var asset = AssetDatabase.LoadAssetAtPath<TSerializableObject>(path);
                if (Editor_Validate((asset, path)) == false) continue;

                var assetRef = new AssetReferenceT<TSerializableObject>(uid);

                assetRef.SetEditorAsset(asset);
                instance.Id = Editor_GetInstanceId(asset);
                instance.Data = assetRef;
                ArrayUtility.Add(ref _maps, instance);
            }
        }

        protected virtual bool Editor_Validate((TSerializableObject asset, string path) data) => true;

        protected virtual TKey Editor_GetInstanceId(TSerializableObject asset) => default(TKey);

        public override Type GetAssetType()
        {
            var type = GetType();
            var genericType = type.BaseType?.GetGenericArguments()[1];
            return genericType;
        }
#endif
    }
}