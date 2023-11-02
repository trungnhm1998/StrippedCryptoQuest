using System;
using System.Collections.Generic;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Character.Hero
{
    /// <summary>
    /// The equipments that the character is wearing
    /// </summary>
    [Serializable]
    public class Equipments
    {
        [field: SerializeField] public List<EquipmentSlot> Slots { get; set; } = new();
    }
}