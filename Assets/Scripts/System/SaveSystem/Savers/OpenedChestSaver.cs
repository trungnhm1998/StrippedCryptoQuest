using CryptoQuest.Gameplay.Loot;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Savers
{
    public class OpenedChestSaver : MonoBehaviour
    {
        [SerializeField] private OpenedChestsSO _openedChests;
        [SerializeField] private SaveSystemSO _saveSystem;

        protected void OnEnable() => _openedChests.Changed += HandleSave;

        protected void OnDisable() => _openedChests.Changed -= HandleSave;

        private void HandleSave() => _saveSystem.SaveData[_openedChests.name] = JsonConvert.SerializeObject(_openedChests.Chests);
    }
}