using System;
using CryptoQuest.Gameplay.Loot;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    [Serializable]
    public class OpenedChestLoader : Loader
    {
        [SerializeField] private OpenedChestsSO _chestSave;
        [SerializeField] private SaveSystemSO _progressionSystem;

        public override void Load()
        {
            _chestSave.Chests.Clear();
            if (!_progressionSystem.SaveData.TryGetValue(_chestSave.name, out var json)) return;
            JsonConvert.PopulateObject(json, _chestSave);
        }
    }
}