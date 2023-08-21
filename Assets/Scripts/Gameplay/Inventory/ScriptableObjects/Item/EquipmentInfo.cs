using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public class EquipmentInfo : ItemInfo
    {
        [field: SerializeField] public EquipmentSO Item { get; private set; }
        [field: SerializeField] public int Level { get; private set; }

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
            inventory.Add(this);
            Activate();
        }

        public void Unequip(InventorySO inventory)
        {
            inventory.Remove(this);
            Activate();
        }
    }

    [Serializable]
    public class StatsValue
    {
        
    }
}