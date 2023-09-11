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
        [Header("Raise Event")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;

        [SerializeField] private RewardDialogEventChannelSO _showRewardDialogEventChannel;

        public void Reward(params LootInfo[] loots)
        {
            foreach (var loot in loots)
                _addLootRequestEventChannel.RaiseEvent(loot);
            if (loots.Length > 0) _showRewardDialogEventChannel.Show(new RewardDialogData(loots));
        }
    }
}