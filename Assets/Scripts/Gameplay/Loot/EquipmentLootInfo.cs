using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Dialogs.RewardDialog;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLootInfo : LootInfo<EquipmentInfo>
    {
        public EquipmentLootInfo(EquipmentInfo item) : base(item) { }

        public override void AddItemToInventory(InventorySO inventory)
        {
            inventory.Add(Item);
        }

        public override UI.Dialogs.RewardDialog.Reward CreateRewardUI()
            => new GenericLocalizedReward(Item.Data.DisplayName);
    }
}