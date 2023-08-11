using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquippingSlotContainer.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type
{
    [CreateAssetMenu(fileName = "Equipment Type", menuName = "Crypto Quest/Inventory/Equipment Type")]
    public class EquipmentTypeSO : GenericItemTypeSO
    {
        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; private set; }
        [field: SerializeField] public EquippingSlotContainer.EType[] AllowedSlots { get; private set; }

        public virtual void Equip(EquipmentInfo equipment, InventorySO inventory)
        {
            inventory.Equip(AllowedSlots[0], equipment);
        }

        public virtual void Unequip(InventorySO inventory)
        {
            inventory.Unequip(AllowedSlots[0]);
        }
    }
}