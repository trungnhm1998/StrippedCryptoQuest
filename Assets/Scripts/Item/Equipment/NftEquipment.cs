using System;
using CryptoQuest.Gameplay.Inventory;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class NftEquipment : EquipmentInfo
    {
        public override bool IsNft => true;
        [field: SerializeField] public string TokenId { get; set; }

        public override bool AddToInventory(IInventoryController inventoryController) =>
            inventoryController.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventoryController) =>
            inventoryController.Remove(this);
    }
}