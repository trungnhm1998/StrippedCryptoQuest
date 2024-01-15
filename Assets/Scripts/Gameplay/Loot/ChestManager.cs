using System.Collections;
using CryptoQuest.Gameplay.NPC.Chest;
using CryptoQuest.Gameplay.Reward;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace CryptoQuest.Gameplay.Loot
{
    public class ChestManager : MonoBehaviour
    {
        [SerializeField] private LootDatabase _lootDatabase;
        [SerializeField] private OpenedChestsSO _openedChests;

        private IRewardManager _rewardManager;

        private void Awake()
        {
            _rewardManager ??= GetComponent<IRewardManager>();
        }

        protected void OnEnable()
        {
            ChestBehaviour.LoadingChest += LoadChest;
            ChestBehaviour.Opening += AddLoots;
        }

        protected void OnDisable()
        {
            ChestBehaviour.LoadingChest -= LoadChest;
            ChestBehaviour.Opening -= AddLoots;
        }

        private void LoadChest(ChestBehaviour chest)
        {
            if (_openedChests.Contains(chest.GUID))
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
            var handle = _lootDatabase.GetHandle(lootId);
            yield return handle;
            if (handle.IsValid() == false || handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load loot with id {lootId}");
                yield break;
            }

            var loots = handle.Result;
            // TODO: This method should be async wait for server to add the loot into inventory first
            _rewardManager.Reward(loots.LootInfos);
            chest.Opened?.Invoke();
            _openedChests.AddChest(chest.GUID);
        }
    }
}