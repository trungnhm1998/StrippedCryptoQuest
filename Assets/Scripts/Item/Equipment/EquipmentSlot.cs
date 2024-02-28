using System;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentSlot
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

        public bool IsValid() => Equipment.IsValid();
    }
}