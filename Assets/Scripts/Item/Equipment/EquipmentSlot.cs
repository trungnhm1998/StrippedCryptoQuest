using System;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentSlot
    {
        public enum EType
        {
            RightHand = 1,
            LeftHand = 2,
            Head = 3,
            Body = 4,
            Leg = 5,
            Foot = 6,
            Accessory1 = 7,
            Accessory2 = 8,
        }

        [field: SerializeField] public EType Type { get; set; }

        [field: SerializeField, SerializeReference]
        public IEquipment Equipment { get; set; }

        public bool IsValid() => Equipment != null && Equipment.IsValid();
    }
}