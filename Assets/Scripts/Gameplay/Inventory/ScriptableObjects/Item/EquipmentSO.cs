using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item
{
    [CreateAssetMenu(fileName = "Equipment Item", menuName = "Crypto Quest/Inventory/Equipment Item")]
    public class EquipmentSO : GenericItem
    {
        [field: Header("Equipment Item")]
        [field: SerializeField] public EquipmentTypeSO EquipmentType { get; protected set; }

        [field: SerializeField] public RaritySO Rarity { get; private set; }
        [field: SerializeField] public LocalizedString LocalizedEquipmentType { get; private set; }
        [field: SerializeField] public int RequiredCharacterLevel { get; private set; }
        [field: SerializeField] public EquipmentSlot.EType[] RequiredSlots { get; private set; }
        public EEquipmentCategory EquipmentCategory => EquipmentType.EquipmentCategory;
    }
}