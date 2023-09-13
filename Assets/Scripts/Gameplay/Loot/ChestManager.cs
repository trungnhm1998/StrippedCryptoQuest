using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.NPC.Chest;
using CryptoQuest.Gameplay.Reward;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ChestSave
    {
        public List<string> OpenedChests = new();
    }

    public class ChestManager : MonoBehaviour
    {
        [SerializeField] private LootDatabase _lootDatabase;
        [SerializeField] private ChestSave _saveData; // TODO: Move this to save manager
        private IRewardManager _rewardManager;

        private void Awake()
        {
            _rewardManager ??= GetComponent<IRewardManager>();
            ChestBehaviour.LoadingChest += LoadChest;
            ChestBehaviour.Opening += AddLoots;
        }

        private void OnDestroy()
        {
            ChestBehaviour.LoadingChest -= LoadChest;
            ChestBehaviour.Opening -= AddLoots;
        }

        private void LoadChest(ChestBehaviour chest)
        {
            if (_saveData.OpenedChests.Contains(chest.GUID))
                chest.Opened?.Invoke();
        }

        private void AddLoots(ChestBehaviour chest)
        {
            if (chest.Treasure == -1) return;
            StartCoroutine(CoAddLoots(chest));
        }

        private IEnumerator CoAddLoots(ChestBehaviour chest)
        {
            var lootId = chest.Treasure;
            yield return _lootDatabase.LoadDataById(lootId);
            var handle = _lootDatabase.GetHandle(lootId );
            yield return handle;
            if (!handle.IsValid() || handle.Status != AsyncOperationStatus.Succeeded) yield break;
            var loots = _lootDatabase.GetDataById(lootId); 
            // TODO: This method should be async wait for server to add the loot into inventory first
            _rewardManager.Reward(loots.LootInfos.ToArray());
            chest.Opened?.Invoke();
            _saveData.OpenedChests.Add(chest.GUID);
        }
    }
}