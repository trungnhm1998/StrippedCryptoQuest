using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public class EquipmentInfo : ItemInfo
    {
        [field: SerializeField] public EquipmentSO Item { get; private set; }

        public bool IsEquipped { get; private set; }

        public EquipmentInfo() { }

        public EquipmentInfo(EquipmentSO itemSO) : base(itemSO)
        {
            Item = itemSO;
        }

        public EquipmentInfo(ItemGenericSO itemSO) : base(itemSO as EquipmentSO) { }
        protected override void Activate() { }

        public void Equip(InventorySO inventory)
        {
            Item.EquipmentType.Equip(this, inventory);
            Activate();
        }

        public void Unequip(InventorySO inventory)
        {
            Item.EquipmentType.Unequip(inventory);
            Activate();
        }
    }
}