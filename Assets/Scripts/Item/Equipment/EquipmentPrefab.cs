using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
{
    /// <summary>
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
        [field: SerializeField] public EquipmentSlot.EType[] RequiredSlots { get; private set; }

        /// <summary>
        /// Allowed slots for this equipment
        /// </summary>
        [field: SerializeField] public EquipmentSlot.EType[] AllowedSlots { get; private set; }

        public EEquipmentCategory EquipmentCategory => EquipmentType.EquipmentCategory;

#if UNITY_EDITOR
        public void Editor_SetEquipmentType(EquipmentTypeSO type) => EquipmentType = type;
        public void Editor_SetRequiredSlots(EquipmentSlot.EType[] requiredSlots) => RequiredSlots = requiredSlots;
#endif
    }
}