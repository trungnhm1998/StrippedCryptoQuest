using System;
using System.Collections.Generic;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    /// <summary>
    /// Use single SO for cross scene inventory
    /// </summary>
    public class InventorySO : ScriptableObject
    {
        [SerializeField, Obsolete] private List<ConsumableInfo> _consumables = new();
        [Obsolete] public List<ConsumableInfo> Consumables => _consumables;

        [field: SerializeReference, SubclassSelector] public List<IEquipment> Equipments { get; private set; } = new();
    }
}