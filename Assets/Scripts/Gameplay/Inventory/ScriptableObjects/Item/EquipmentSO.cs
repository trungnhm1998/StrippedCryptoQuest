using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Equipment Item", menuName = "Crypto Quest/Inventory/Equipment Item")]
    public class EquipmentSO : ItemGenericSO
    {
        [Header("Equipment Item")]
        [SerializeField] private EquipmentTypeSO _equipmentType;

        [SerializeField] private RaritySO _rarity;

        [field: SerializeField] public bool IsNftItem { get; private set; }
        public EquipmentTypeSO EquipmentType => _equipmentType;
        public RaritySO Rarity => _rarity;
    }
}