using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container
{
    [Serializable]
    public class InventoryContainer
    {
        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; set; }
        [field: SerializeField] public List<EquipmentInfo> CurrentItems { get; set; }
    }
}