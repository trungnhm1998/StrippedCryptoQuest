using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class UsableLootInfo : LootInfo<UsableInfo>
    {
        public UsableLootInfo(UsableInfo item) : base(item) { }
        public override void AddItemToInventory(InventorySO inventory) => inventory.Add(Item);
        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() => new ConsumableReward(this);
        public override LootInfo Clone() => new UsableLootInfo(Item.Clone());
        public override bool AcceptMerger(IRewardMerger merger) => merger.Visit(this);
        public override bool Merge(IRewardMerger merger) => merger.Merge(this);

        public void Merge(UsableLootInfo loot)
        {
            Item.SetQuantity(Item.Quantity + loot.Item.Quantity);
        }
    }
}