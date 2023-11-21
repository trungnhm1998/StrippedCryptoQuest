using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class CurrencyLootInfo : LootInfo<CurrencyInfo>
    {
        public CurrencyLootInfo(CurrencyInfo item) : base(item) { }
        public override void AddItemToInventory(IInventoryController inventory) => Item.AddToInventory(inventory);

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() =>
            new AmountReward(Item.Amount, Item.Data.DisplayName);

        public override LootInfo Clone() => new CurrencyLootInfo(Item.Clone());
        public override bool AcceptMerger(IRewardMerger merger) => merger.Visit(this);
        public override bool Merge(IRewardMerger merger) => merger.Merge(this);
        public void Merge(CurrencyLootInfo loot) => Item.UpdateCurrencyAmount(loot.Item.Amount);
    }
}