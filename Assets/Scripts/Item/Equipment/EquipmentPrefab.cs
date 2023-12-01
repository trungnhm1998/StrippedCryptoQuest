using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    /// <summary>
    /// Config/Prefab of the equipment
    /// 
    /// New equipment with different will create from this
    /// </summary>
    [CreateAssetMenu(fileName = "Equipment Item", menuName = "Crypto Quest/Inventory/Equipment Item")]
    public class EquipmentPrefab : GenericItem
    {
        [field: Header("Equipment Item")]
        [field: SerializeField] public EquipmentTypeSO EquipmentType { get; protected set; }

        /// <summary>
        /// This equipment will occupy these slots
        ///
        /// Leave this empty for Accessories
        /// </summary>
        [field: SerializeField] public ESlot[] RequiredSlots { get; private set; }

        /// <summary>
        /// Allowed slots for this equipment
        /// </summary>
        [field: SerializeField] public ESlot[] AllowedSlots { get; private set; }

        public EEquipmentCategory EquipmentCategory => EquipmentType.EquipmentCategory;
    }
}