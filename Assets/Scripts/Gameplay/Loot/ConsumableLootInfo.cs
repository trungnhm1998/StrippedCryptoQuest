using System;
using CryptoQuest.Battle;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class ConsumableLootInfo : LootInfo<ConsumableInfo>
    {
        public ConsumableLootInfo() { }
        public ConsumableLootInfo(ConsumableInfo item) : base(item) { }

        // public override UI.Dialogs.RewardDialog.Reward CreateRewardUI() => new ConsumableReward(this);
        public override LootInfo Clone() => new ConsumableLootInfo(new ConsumableInfo(Item.Data, Item.Quantity));

        public override void Accept(ILootVisitor lootController) => lootController.Visit(this);
        public override bool TryMerge(ILootMerger lootMerger) => lootMerger.Merge(this);
        public override string Name => Item.DisplayName.GetLocalizedString();
        public override void Accept(UIRewardItem rewardUI) => rewardUI.Visit(this);
    }
}