using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;
using CryptoQuest.Events;

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
        [Header("Raise Event")]
        [SerializeField] private LootEventChannelSO _addLootRequestEventChannel;
        
        public void Reward(LootInfo loot)
        {
            _addLootRequestEventChannel.RaiseEvent(loot);
        }
    }
}