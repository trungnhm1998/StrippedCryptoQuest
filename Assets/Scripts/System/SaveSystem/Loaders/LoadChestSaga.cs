using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.SaveSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.System.SaveSystem.Loaders
{
    public class LoadChestSaga : MonoBehaviour, ILoader
    {
        [SerializeField] private OpenedChestsSO _openedChests;

        public IEnumerator Load(SaveSystemSO progressionSystem)
        {
            if (progressionSystem.SaveData.TryGetValue(_openedChests.name, out var json))
                _openedChests.Chests = JsonConvert.DeserializeObject<List<string>>(json);
            yield break;
        }
    }
}