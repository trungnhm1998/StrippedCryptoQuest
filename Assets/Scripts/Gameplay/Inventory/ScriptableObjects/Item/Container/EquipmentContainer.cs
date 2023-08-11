using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container
{
    [Serializable]
    public class EquipmentContainer
    {
        [field: SerializeField] public EquipmentTypeSO Type { get; private set; }
        [field: SerializeField] public EquipmentInfo[] EquipmentInfos { get; private set; }
    }
}