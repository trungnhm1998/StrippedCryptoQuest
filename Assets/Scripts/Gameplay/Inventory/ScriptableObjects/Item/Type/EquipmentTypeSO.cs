using System;
using CryptoQuest.Gameplay.Character;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using SlotType = CryptoQuest.Item.Equipment.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type
{
    [CreateAssetMenu(fileName = "Equipment Type", menuName = "Crypto Quest/Inventory/Equipment Type")]
    public class EquipmentTypeSO : GenericItemTypeSO
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; private set; }
        [field: SerializeField] public SlotType[] AllowedSlots { get; private set; } = Array.Empty<SlotType>();
        [field: SerializeField] public CharacterClass[] AllowedClasses { get; private set; } = Array.Empty<CharacterClass>();

        public virtual void Equip(EquipmentInfo equipment, InventorySO inventory) { }

        public virtual void Unequip(InventorySO inventory) { }
    }
}