using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public abstract class LootInfo
    {
        public abstract void AddItemToInventory(InventorySO inventory);

        public abstract UI.Dialogs.RewardDialog.Reward CreateRewardUI();
        public abstract LootInfo Clone();

        public abstract bool Merge(LootInfo otherLoot);
    }

    [Serializable]
    public abstract class LootInfo<TItemInfo> : LootInfo where TItemInfo : ItemInfo
    {
        [SerializeField] private TItemInfo _item;

        public TItemInfo Item
        {
            get => _item;
            set => _item = value;
        }
        public LootInfo(TItemInfo item) => _item = item;
    }
}