using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;

namespace CryptoQuest.Gameplay.Loot
{
    [Serializable]
    public class EquipmentLootInfo : LootInfo<EquipmentInfo>
    {
        public override void AddItemToInventory(InventorySO inventory)
        {
            inventory.Add(Item);
        }

        public EquipmentLootInfo(EquipmentInfo item) : base(item) { }
    }
}