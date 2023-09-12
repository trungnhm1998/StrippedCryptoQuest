using System;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Reward
{
    public interface IRewardManager
    {
        /// <summary>
        /// Reward player with item. could be <see cref="EquipmentInfo"/> or <see cref="UsableInfo"/> or <see cref="CurrencyInfo"/>
        /// </summary>
        public void Reward(params LootInfo[] loots);
    }

    public class RewardManager : MonoBehaviour, IRewardManager
    {
        public delegate void RewardEvent(params LootInfo[] loots);

        public static event RewardEvent Rewarding;
        public static void RewardPlayer(params LootInfo[] loots) => Rewarding?.Invoke(loots);

        [Header("Raise Event")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        [SerializeField] private RewardDialogEventChannelSO _showRewardDialogEventChannel;

        public void Reward(params LootInfo[] loots)
        {
            foreach (var loot in loots)
                _addLootRequestEventChannel.RaiseEvent(loot);
            if (loots.Length > 0) _showRewardDialogEventChannel.Show(new RewardDialogData(loots));
        }

        public LootInfo[] MergeLoots(params LootInfo[] loots)
        {
            List<LootInfo> mergedLoots = new(loots);
            for (var index = 0; index < mergedLoots.Count; index++)
            {
                var loot = mergedLoots[index];
                MergeThenRemoveOtherLoot(ref mergedLoots, currentLootIndex: index, ref loot);
            }

            return mergedLoots.ToArray();
        }

        private static void MergeThenRemoveOtherLoot(ref List<LootInfo> loots, int currentLootIndex, ref LootInfo loot)
        {
            for (int nextLootIndex = currentLootIndex + 1; nextLootIndex < loots.Count;)
            {
                if (loot.Merge(loots[nextLootIndex]))
                    loots.RemoveAt(nextLootIndex);
                else
                    nextLootIndex++;
            }
        }
    }
}