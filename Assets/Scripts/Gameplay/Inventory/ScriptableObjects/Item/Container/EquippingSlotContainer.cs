using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container
{
    [Serializable]
    public class EquippingSlotContainer
    {
        public enum EType
        {
            Weapon = 0,
            Shield = 1,
            Head = 2,
            Body = 3,
            Leg = 4,
            Foot = 5,
            Accessory1 = 6,
            Accessory2 = 7,
        }

        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; set; }
        [field: SerializeField] public EType Type { get; set; }
        [field: SerializeField] public EquipmentInfo Equipment { get; set; }

        public void UpdateEquipment(EquipmentInfo equipment)
        {
            Equipment = equipment;
        }
    }
}