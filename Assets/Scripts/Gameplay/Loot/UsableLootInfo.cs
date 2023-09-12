using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class UsableLootInfo : LootInfo<UsableInfo>
    {
        public UsableLootInfo(UsableInfo item) : base(item) { }
        public override void AddItemToInventory(InventorySO inventory) => inventory.Add(Item);
        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() => new ConsumableReward(this);
        public override LootInfo Clone() => new UsableLootInfo(Item);
        public override bool Merge(LootInfo otherLoot)
        {
            if (otherLoot is not UsableLootInfo usableLoot) return false;
            if (Item.Data != usableLoot.Item.Data) return false;
            Item.SetQuantity(Item.Quantity + usableLoot.Item.Quantity);
            return true;
        }
    }
}