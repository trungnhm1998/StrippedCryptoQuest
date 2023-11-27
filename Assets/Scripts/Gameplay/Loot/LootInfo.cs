using System;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory;
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
        public abstract UI.Dialogs.RewardDialog.Reward CreateRewardUI();
        public abstract LootInfo Clone();
        public abstract bool IsValid();
        public abstract void Accept(ILootVisitor lootController);
        public abstract bool TryMerge(ILootMerger lootMerger);
    }

    [Serializable]
    public abstract class LootInfo<TItemInfo> : LootInfo where TItemInfo : ItemInfo
    {
        [SerializeField] private TItemInfo _item;
        public TItemInfo Item => _item;
        protected LootInfo() { }
        protected LootInfo(TItemInfo item) => _item = item;

        public override bool IsValid() => _item != null && _item.IsValid();
    }
}