using System.Collections.Generic;
using CryptoQuest.Data.Item;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public List<ExpendableItemInfo> Items;

        public List<UsableSO> UsableItem;
        public List<EquipmentSO> EquipmentItem;
    }
}