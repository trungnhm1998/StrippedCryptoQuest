using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward.Events;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Dialogs.RewardDialog;
using UnityEngine;

namespace CryptoQuest.Gameplay.Reward
{
    public interface IRewardManager
    {
        /// <summary>
        /// Reward player with item. could be <see cref="EquipmentInfo"/> or <see cref="ConsumableInfo"/> or <see cref="CurrencyInfo"/>
        /// </summary>
        public void Reward(List<LootInfo> loots);
    }

    public class RewardManager : MonoBehaviour, IRewardManager
    {
        [Header("Listen Event")]
        [SerializeField] private RewardLootEvent _rewardLootEvent;

        [Header("Raise Event")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;
        [SerializeField] private RewardDialogEventChannelSO _showRewardDialogEventChannel;

        private void Awake() => _rewardLootEvent.EventRaised += Reward;

        private void OnDestroy() => _rewardLootEvent.EventRaised -= Reward;

        public void Reward(List<LootInfo> loots)
        {
            if (loots == null || loots.Count == 0) return;
            foreach (var loot in loots)
                _addLootRequestEventChannel.RaiseEvent(loot);
            _showRewardDialogEventChannel.Show(new RewardDialogData(loots));
        }
    }
}