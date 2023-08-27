using CryptoQuest.Gameplay.Inventory.Items;
using UnityEngine;
using SlotType = CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type
{
    [CreateAssetMenu(fileName = "Equipment Type", menuName = "Crypto Quest/Inventory/Equipment Type")]
    public class EquipmentTypeSO : GenericItemTypeSO
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; private set; }
        [field: SerializeField] public SlotType[] AllowedSlots { get; private set; }

        public virtual void Equip(EquipmentInfo equipment, InventorySO inventory) { }

        public virtual void Unequip(InventorySO inventory) { }
    }
}