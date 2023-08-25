using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentSlot : MonoBehaviour
    {
        [field: SerializeField] public EquipmentSlot.EType SlotType { get; private set; }
        [SerializeField] private UIEquipment _equipment;

        public void Init(EquipmentInfo equipmentSlotEquipment)
        {
            _equipment.gameObject.SetActive(true);
            _equipment.Init(equipmentSlotEquipment);
        }

        public void Reset()
        {
            _equipment.gameObject.SetActive(false);
        }
    }
}