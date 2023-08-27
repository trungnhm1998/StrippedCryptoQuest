using System;
using CryptoQuest.Gameplay.Inventory.Items;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container
{
    [Serializable]
    public struct EquipmentSlot
    {
        public enum EType
        {
            LeftHand = 0,
            RightHand = 1,
            Head = 2,
            Body = 3,
            Leg = 4,
            Foot = 5,
            Accessory1 = 6,
            Accessory2 = 7,
        }

        [field: SerializeField] public EType Type { get; set; }
        [field: SerializeField] public EquipmentInfo Equipment { get; set; }

        public bool IsValid()
        {
            return Equipment.IsValid();
        }
    }
}