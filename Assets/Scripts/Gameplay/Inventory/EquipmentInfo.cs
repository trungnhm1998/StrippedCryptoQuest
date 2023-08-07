using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory
{
    [Serializable]
    public class EquipmentInfo : ItemInfo
    {
        [field: SerializeField] public EquipmentSO Item { get; private set; }

        public bool IsEquipped { get; private set; }
        public EquipmentInfo() { }
        public EquipmentInfo(EquipmentSO itemSo) : base(itemSo) { }
        protected override void Activate() { }

        public void Equip() => Activate();
    }
}