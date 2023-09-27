using System;
using CryptoQuest.Gameplay.Character;
using UnityEngine;
using SlotType = CryptoQuest.Item.Equipment.EquipmentSlot.EType;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type
{
    [CreateAssetMenu(fileName = "Equipment Type", menuName = "Crypto Quest/Inventory/Equipment Type")]
    public class EquipmentTypeSO : GenericItemTypeSO
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public EEquipmentCategory EquipmentCategory { get; private set; }

        [field: SerializeField] public CharacterClass[] AllowedClasses { get; private set; } =
            Array.Empty<CharacterClass>();
    }
}