using System;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public event Action<UIEquipmentItem> Deselected;
        public event Action<UIEquipmentItem> Inspecting;
        public event Action<UIEquipmentItem> EquipItem;
        [SerializeField] private UIEquipment _equipmentUI;
        public IEquipment Equipment => _equipmentUI.Equipment;
        private bool _canClick;

        public void OnSelect(BaseEventData eventData)
        {
            if (!_equipmentUI.Equipment.IsValid() || !_canClick) return;
            Inspecting?.Invoke(this);
        }

        public void Init(IEquipment equipment)
        {
            _canClick = true;
            _equipmentUI.Init(equipment);
        }

        public void DeactivateButton()
        {
            _canClick = false;
            _equipmentUI.DisableButton();
        }

        public void OnEquip()
        {
            if (!_canClick) return;
            EquipItem?.Invoke(this);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            if (!_canClick) return;
            Deselected?.Invoke(this);
        }
    }
}