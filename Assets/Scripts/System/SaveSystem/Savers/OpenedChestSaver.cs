using System;
using CryptoQuest.Gameplay.Loot;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    [Serializable]
    public class OpenedChestSaver : SaverBase
    {
        [SerializeField] private OpenedChestsSO _chestSave;

        public override void RegistEvents()
        {
            _chestSave.Changed += SaveChest;
        }

        public override void UnregistEvents()
        {
            _chestSave.Changed -= SaveChest;
        }

        private void SaveChest()
        {
            _saveSystem.SaveData[_chestSave.name] = JsonConvert.SerializeObject(_chestSave);
            _saveHandler.Save();
        }
    }
}