using System;
using CryptoQuest.Map;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    /// <summary>
    /// To handle save player last teleport path and spawn at correct position after load game
    /// </summary>
    [Serializable]
    public class PathStorageSaver : SaverBase
    {
        [SerializeField] private PathStorageSO _pathStorage;

        public override void RegistEvents() => _pathStorage.LastTakenPathChanged += SaveLastTakenPath;
        public override void UnregistEvents() => _pathStorage.LastTakenPathChanged -= SaveLastTakenPath;

        private void SaveLastTakenPath(MapPathSO mapPath)
        {
            _saveSystem.SaveData[_pathStorage.name] = mapPath.Guid;
            // Only need save local since scene loader will upload to server
            _saveSystem.Save();
        }
    }
}