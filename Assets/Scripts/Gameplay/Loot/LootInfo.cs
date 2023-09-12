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
    }

    [Serializable]
    public abstract class LootInfo<TItemInfo> : LootInfo where TItemInfo : ItemInfo
    {
        [SerializeField] private TItemInfo _item;

        public TItemInfo Item => _item;

        public LootInfo(TItemInfo item) => _item = item;
    }
}