using System;
using System.Collections.Generic;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Item.MagicStone;
using UnityEngine;
using ESlotType =
    CryptoQuest.Item.Equipment.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    /// <summary>
    /// Use single SO for cross scene inventory
    /// </summary>
    public class InventorySO : ScriptableObject
    {
        [SerializeField] private List<ConsumableInfo> _consumables = new();
        public List<ConsumableInfo> Consumables => _consumables;

        [field: SerializeField] public List<Equipment> Equipments { get; set; } = new();
        [field: SerializeField] public List<NftEquipment> NftEquipments { get; set; } = new();
        [field: SerializeField] public List<MagicStoneInfo> MagicStones { get; set; } = new();
    }
}