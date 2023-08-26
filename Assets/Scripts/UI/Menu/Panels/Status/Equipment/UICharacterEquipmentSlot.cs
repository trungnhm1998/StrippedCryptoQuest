using System;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Container;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    public class UICharacterEquipmentSlot : MonoBehaviour
    {
        public event Action<EquipmentSlot.EType> ChangingEquipment;
        [field: SerializeField] public EquipmentSlot.EType SlotType { get; private set; }
        public ITooltip Tooltip { get; set; }

        [SerializeField] private UIEquipment _equipment;

        private void Awake()
        {
            _equipment.Tooltip = Tooltip;
        }

        public void Init(EquipmentInfo equipmentSlotEquipment)
        {
            _equipment.gameObject.SetActive(true);
            _equipment.Init(equipmentSlotEquipment);
        }

        public void Reset()
        {
            _equipment.gameObject.SetActive(false);
        }

        public void OnChangingEquipment()
        {
            ChangingEquipment?.Invoke(SlotType);
        }
    }
}