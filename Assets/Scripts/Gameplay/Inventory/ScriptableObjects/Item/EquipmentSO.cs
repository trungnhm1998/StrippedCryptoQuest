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
        
        protected EquipmentTypeSO EquipmentType
        {
            get => _equipmentType;
            set => _equipmentType = value;
        }

        public RaritySO Rarity
        {
            get => _rarity;
            set => _rarity = value;
        }
    }
}