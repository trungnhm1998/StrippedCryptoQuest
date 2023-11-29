using System;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class Equipment : EquipmentInfo
    {
        public override bool IsNft => false;

        public override bool ContainedInInventory(IInventoryController inventoryController) =>
            inventoryController.Contains(this);

        public override bool AddToInventory(IInventoryController inventoryController) =>
            inventoryController.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventoryController) =>
            inventoryController.Remove(this);
    }
}