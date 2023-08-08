using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        [field: SerializeField] public List<UsableInfo> UsableItems { get; private set; }
        [field: SerializeField] public List<EquipmentInfo> Equipments { get; private set; }
        [field: SerializeField] public List<WeaponInfo> Weapons { get; private set; }
    }
}