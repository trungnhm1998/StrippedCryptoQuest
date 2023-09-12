using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class CurrencyLootInfo : LootInfo<CurrencyInfo>
    {
        public CurrencyLootInfo(CurrencyInfo item) : base(item) { }
        public override void AddItemToInventory(InventorySO inventory) => inventory.Add(Item);
        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() =>
            new AmountReward(Item.Amount, Item.Data.DisplayName);
        public override LootInfo Clone() => new CurrencyLootInfo(Item);
        public override bool Merge(LootInfo otherLoot)
        {
            if (otherLoot is not CurrencyLootInfo currencyLoot) return false;
            if (Item.Data != currencyLoot.Item.Data) return false;
            Item.UpdateCurrencyAmount(currencyLoot.Item.Amount);
            return true;
        }
    }
}