using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Config
{
    public class InventoryConfigSO : SerializableScriptableObject
    {
        [field: SerializeField, Tooltip("The number of equipment category index. default: 8")]
        public int CategorySlotIndex { get; private set; } = 8;

        [field: SerializeField, Tooltip("The number of inventory slots. default: 7")]
        public int SlotTypeIndex { get; private set; } = 7;
    }
}