using System;
using CryptoQuest.Gameplay.Inventory;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class Equipment : EquipmentInfo
    {
        [SerializeField] private EquipmentSO _equipmentSO;

        public override EquipmentData Data => _equipmentSO.Data;

        public override bool IsNftItem => false;

        public override bool ContainedInInventory(IInventoryController inventoryController) =>
            inventoryController.Contains(this);

        public override bool AddToInventory(IInventoryController inventoryController) => inventoryController.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventoryController) =>
            inventoryController.Remove(this);
    }
}