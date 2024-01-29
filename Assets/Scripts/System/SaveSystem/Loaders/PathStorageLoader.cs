using System;
using System.Collections;
using CryptoQuest.Map;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class PathStorageLoader : Loader
    {
        [SerializeField] private PathStorageSO _pathStorage;
        [SerializeField] private SaveSystemSO _progressionSystem;

        public override IEnumerator LoadAsync()
        {
            _pathStorage.LastTakenPath = null;
            
            if (!_progressionSystem.SaveData.TryGetValue(_pathStorage.name, out var json))
                yield break;

            var handle = Addressables.LoadAssetAsync<MapPathSO>(json);
            yield return handle;

            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                _pathStorage.LastTakenPath = handle.Result;
            }
        }
    }
}