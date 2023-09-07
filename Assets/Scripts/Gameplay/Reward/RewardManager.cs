using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Reward
{
    public interface IRewardManager
    {
        /// <summary>
        /// Reward player with item. could be <see cref="EquipmentInfo"/> or <see cref="UsableInfo"/> or <see cref="CurrencyInfo"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        void Reward(LootInfo item);
    }

    public class RewardManager : MonoBehaviour, IRewardManager
    {
        [SerializeField] private ServiceProvider _serviceProvider;
        
        public void Reward(LootInfo item)
        {
            item.AddItemToInventory(_serviceProvider.Inventory);
        }
    }
}