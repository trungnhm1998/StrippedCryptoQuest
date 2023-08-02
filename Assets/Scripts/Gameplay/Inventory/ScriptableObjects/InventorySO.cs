using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Crypto Quest/Inventory/Inventory")]
    public class InventorySO : ScriptableObject
    {
        public List<ExpendableItemInfo> Items;
    }
}