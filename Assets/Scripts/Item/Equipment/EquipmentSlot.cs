using System;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    [Serializable]
    public class EquipmentSlot
    {
        [field: SerializeField] public ESlot Type { get; set; }

        [field: SerializeField, SerializeReference]
        public IEquipment Equipment { get; set; }

        public bool IsValid() => Equipment != null && Equipment.IsValid();
    }

    [Serializable]
    public enum ESlot
    {
        None = 0,
        RightHand = 1,
        LeftHand = 2,
        Head = 3,
        Body = 4,
        Leg = 5,
        Foot = 6,
        Accessory1 = 7,
        Accessory2 = 8,
    }
}