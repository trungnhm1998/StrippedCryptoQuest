using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLootInfo : LootInfo<EquipmentInfo>
    {
        public EquipmentLootInfo(EquipmentInfo item) : base(item) { }
        public override void AddItemToInventory(InventorySO inventory) => inventory.Add(Item);

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
            => new GenericLocalizedReward(Item.DisplayName);

        public override LootInfo Clone() => new EquipmentLootInfo(Item.Clone());

        /// <summary>
        ///  equipment loot can't be merged
        /// </summary>
        /// <param name="merger"></param>
        /// <returns></returns>
        public override bool AcceptMerger(IRewardMerger merger) => true;
        public override bool Merge(IRewardMerger merger) => false;
    }
}