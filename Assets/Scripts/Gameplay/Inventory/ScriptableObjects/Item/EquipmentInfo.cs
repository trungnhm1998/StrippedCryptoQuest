using System;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [Serializable]
    public class EquipmentInfo : ItemInfo<EquipmentSO>
    {
        [field: SerializeField] public int Level { get; private set; }
        [field: SerializeField] public StatsDef Stats { get; private set; }

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
}