using System;
using CryptoQuest.Character;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Item.Equipment
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