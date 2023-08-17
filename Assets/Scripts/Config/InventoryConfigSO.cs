using UnityEngine;

namespace CryptoQuest.Config
{
    public class InventoryConfigSO : ScriptableObject
    {
        [field: SerializeField, Tooltip("The number of equipment slots. default: 8")]
        public int EquipmentSlot { get; private set; } = 8;

        [field: SerializeField, Tooltip("The number of inventory slots. default: 7")]
        public int InventorySlot { get; private set; } = 7;
    }
}