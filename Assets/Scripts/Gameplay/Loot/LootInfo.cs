using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Loot
{
    /// <summary>
    ///  need to be abstract class so we could serialize it in unity
    /// </summary>
    [Serializable]
    public abstract class LootInfo
    {
        public abstract void AddItemToInventory(IInventoryController inventory);

        public abstract UI.Dialogs.RewardDialog.Reward CreateRewardUI();
        public abstract LootInfo Clone();

        public abstract bool AcceptMerger(IRewardMerger merger);

        public abstract bool Merge(IRewardMerger merger);
        public abstract bool IsValid();
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

        public override bool IsValid() => _item != null && _item.IsValid();
    }
}