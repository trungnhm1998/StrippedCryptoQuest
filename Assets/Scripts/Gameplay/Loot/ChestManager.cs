using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.NPC.Chest;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.System.SaveSystem.Actions;
using UnityEngine;

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

        [HideInInspector] public ChestSave SaveData;
        private TinyMessenger.TinyMessageSubscriptionToken _listenToLoadCompletedEventToken;

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

        private void Start()
        {
            _listenToLoadCompletedEventToken = ActionDispatcher.Bind<LoadChestCompletedAction>(_ => LoadChest());
            ActionDispatcher.Dispatch(new LoadChestAction(this));
        }

        private void LoadChest()
        {
            ActionDispatcher.Unbind(_listenToLoadCompletedEventToken);
            // TODO: save data has restored, not sure how to handle it
        }

        private void LoadChest(ChestBehaviour chest)
        {
            if (SaveData.OpenedChests.Contains(chest.GUID))
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
            if (handle.IsValid() == false || handle.IsDone == false || handle.Result == null)
            {
                Debug.LogError($"Failed to load loot with id {lootId}");
                yield break;
            }

            var loots = handle.Result;
            // TODO: This method should be async wait for server to add the loot into inventory first
            _rewardManager.Reward(loots.LootInfos);
            chest.Opened?.Invoke();
            SaveData.OpenedChests.Add(chest.GUID);
            ActionDispatcher.Dispatch(new SaveChestAction(this));
        }
    }
}