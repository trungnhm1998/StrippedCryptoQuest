using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Item;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ConsumableLootInfo : LootInfo<ConsumableInfo>
    {
        public ConsumableLootInfo() { }
        public ConsumableLootInfo(ConsumableInfo item) : base(item) { }
        public override void AddItemToInventory(IInventoryController inventory) => Item.AddToInventory(inventory);

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() => new ConsumableReward(this);
        public override LootInfo Clone() => new ConsumableLootInfo(new ConsumableInfo(Item.Data, Item.Quantity));

        public override bool AcceptMerger(IRewardMerger merger) => merger.Visit(this);
        public override bool Merge(IRewardMerger merger) => merger.Merge(this);

        public void Merge(ConsumableLootInfo loot)
        {
            Item.SetQuantity(Item.Quantity + loot.Item.Quantity);
        }
    }
}