using System;
using CryptoQuest.Gameplay.Inventory;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class NftEquipment : EquipmentInfo
    {
        [field: SerializeField] public uint Id { get; set; }
        [field: SerializeField] public string TokenId { get; set; }
        [field: SerializeField] public EquipmentData Def { get; set; }
        public override EquipmentData Data => Def;

        public override bool AddToInventory(IInventoryController inventoryController) =>
            inventoryController.Add(this);

        public override bool RemoveFromInventory(IInventoryController inventoryController) =>
            inventoryController.Remove(this);

        public override bool IsNft => true;

        public override bool ContainedInInventory(IInventoryController inventoryController) =>
            inventoryController.Contains(this);
    }
}