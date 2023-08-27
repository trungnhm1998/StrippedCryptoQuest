using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentSlot.EType> ShowEquipmentsInventoryWithType;
        [field: SerializeField] public EquipmentSlot.EType SlotType { get; private set; }

        [SerializeField] private UIEquipment _equipment;

        public void Init(EquipmentInfo equipmentSlotEquipment)
        {
            _equipment.Init(equipmentSlotEquipment);
            _equipment.gameObject.SetActive(true);
        }

        public void Reset()
        {
            _equipment.Reset();
            _equipment.gameObject.SetActive(false);
        }

        public void OnChangingEquipment()
        {
            ShowEquipmentsInventoryWithType?.Invoke(SlotType);
        }
    }
}