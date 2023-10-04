using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Battle.Components;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.PlayerParty;
using CryptoQuest.Gameplay.Reward.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.System;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Reward
{
    public interface IRewardManager
    {
        /// <summary>
        /// Reward player with item. could be <see cref="EquipmentInfo"/> or <see cref="ConsumableInfo"/> or <see cref="CurrencyInfo"/>
        /// </summary>
        public void Reward(params LootInfo[] loots);
    }

    public class RewardManager : MonoBehaviour, IRewardManager
    {
        public delegate void RewardEvent(params LootInfo[] loots);

        public static event Action<float> RewardExpEvent;
        public static void RewardPlayerExp(float exp) => RewardExpEvent?.Invoke(exp);

        [Header("Raise Event")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;
        [SerializeField] private RewardDialogEventChannelSO _showRewardDialogEventChannel;
        [SerializeField] private RewardSO _rewardEventChannel;

        private IPartyController _party;

        private void Awake()
        {
            _rewardEventChannel.OnRewardEvent += Reward;
            _rewardEventChannel.OnRewardExpEvent += RewardExp;
        }

        private void Start()
        {
            _party = ServiceProvider.GetService<IPartyController>();
        }

        private void OnDestroy()
        {
            _rewardEventChannel.OnRewardEvent -= Reward;
            _rewardEventChannel.OnRewardExpEvent -= RewardExp;
        }

        public void Reward(params LootInfo[] loots)
        {
            if (loots == null || loots.Length == 0) return;
            var mergedLoots = CloneAndMergeLoots(loots);
            foreach (var loot in mergedLoots)
                _addLootRequestEventChannel.RaiseEvent(loot);
            _showRewardDialogEventChannel.Show(new RewardDialogData(mergedLoots));
        }

        private void RewardExp(float exp)
        {
            foreach (var slot in _party.Slots)
            {
                if (!slot.IsValid()) continue;
                var levelSystem = slot.HeroBehaviour.GetComponent<LevelSystem>(); // Force error hero needed this
                levelSystem.AddExp(exp);
            }
        }

        /// <summary>
        /// Except equipments, merge all loots with same item data
        /// e.g. 2x <see cref="CurrencyInfo"/> with same <see cref="CurrencySO"/> will be merged into 1x <see cref="CurrencyInfo"/> with amount 2
        /// </summary>
        /// <param name="loots">Contains <see cref="CurrencyLootInfo"/>, <see cref="ExpLoot"/>, <see cref="UsableLootInfo"/>, <see cref="EquipmentLootInfo"/></param>
        /// <returns>Loots that merged and cloned to be add into inventory</returns>
        public static LootInfo[] CloneAndMergeLoots(params LootInfo[] loots)
        {
            List<LootInfo> mergedLoots = loots.Select(loot => loot.Clone()).ToList();
            IRewardMerger rewardMerger = new BasicMerger(ref mergedLoots);
            var currentLootIndex = 0;
            while (currentLootIndex < mergedLoots.Count)
            {
                var loot = mergedLoots[currentLootIndex];
                if (loot.AcceptMerger(rewardMerger))
                    currentLootIndex++;
            }

            return mergedLoots.ToArray();
        }
    }
}