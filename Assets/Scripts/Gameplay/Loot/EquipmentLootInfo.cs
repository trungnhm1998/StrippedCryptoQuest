using System;
using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLootInfo : LootInfo<EquipmentInfo>
    {
        public EquipmentLootInfo(EquipmentInfo item) : base(item) { }
        public override void AddItemToInventory(IInventoryController inventory) => Item.AddToInventory(inventory);

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
            => new GenericLocalizedReward(Item.DisplayName);

        public override LootInfo Clone() => new EquipmentLootInfo(Item.Clone() as EquipmentInfo);

        /// <summary>
        ///  equipment loot can't be merged
        /// </summary>
        /// <param name="merger"></param>
        /// <returns></returns>
        public override bool AcceptMerger(IRewardMerger merger) => true;
        public override bool Merge(IRewardMerger merger) => false;
    }
}