using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Gameplay.NPC.Chest;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.System;
using CryptoQuest.System.SaveSystem;
using IndiGames.Core.SaveSystem;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ChestSave
    {
        public List<string> OpenedChests = new();
    }

    public class ChestManager : MonoBehaviour, ISaveObject
    {
        [SerializeField] private LootDatabase _lootDatabase;
        [SerializeField] private ChestSave _saveData; // TODO: Move this to save manager
        private IRewardManager _rewardManager;
        private ISaveSystem _saveSystem;

        private void Awake()
        {
            _rewardManager ??= GetComponent<IRewardManager>();
            _saveSystem = ServiceProvider.GetService<ISaveSystem>();
            ChestBehaviour.LoadingChest += LoadChest;
            ChestBehaviour.Opening += AddLoots;
        }

        private void OnDestroy()
        {
            ChestBehaviour.LoadingChest -= LoadChest;
            ChestBehaviour.Opening -= AddLoots;
        }

        private void Start()
        {
            _saveSystem?.LoadObject(this);
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
            _saveData.OpenedChests.Add(chest.GUID);
            _saveSystem?.SaveObject(this);
        }

        #region SaveSystem        
        public string Key { get { return "Chest"; } }

        public string ToJson()
        {
            return JsonUtility.ToJson(_saveData);
        }

        public bool FromJson(string json)
        {
            if (!string.IsNullOrEmpty(json)) 
            {
                JsonUtility.FromJsonOverwrite(json, _saveData);
                return true;
            }
            return false;
        }
        #endregion
    }
}