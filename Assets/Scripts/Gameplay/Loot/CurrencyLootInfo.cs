using System;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class CurrencyLootInfo : LootInfo<CurrencyInfo>
    {
        public CurrencyLootInfo() { }

        public CurrencyLootInfo(CurrencyInfo item) : base(item) { }
        // public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() =>
        //     new AmountReward(Item.Amount, Item.Data.DisplayName);

        public override bool IsItem => false;
        public override LootInfo Clone() => new CurrencyLootInfo(new CurrencyInfo(Item.Data, Item.Amount));

        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);
        public override bool TryMerge(ILootMerger lootMerger) => lootMerger.Merge(this);
        public override void Accept(UIRewardItem rewardUI)
        {
            rewardUI.Visit(this);
        }
    }
}