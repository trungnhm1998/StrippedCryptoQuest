using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Dialogs.RewardDialog;
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
    public class ExpLoot : LootInfo
    {
        [field: SerializeField] public float Exp { get; private set; }
        public ExpLoot(float experiencePoints) => Exp = experiencePoints;

        public override void AddItemToInventory(InventorySO inventory) { }

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
            => new GenericReward($"{Exp} EXP");

        public override LootInfo Clone() => new ExpLoot(Exp);
        public override bool Merge(LootInfo otherLoot)
        {
            if (otherLoot is not ExpLoot expLoot) return false;
            Exp += expLoot.Exp;
            return true;
        }
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