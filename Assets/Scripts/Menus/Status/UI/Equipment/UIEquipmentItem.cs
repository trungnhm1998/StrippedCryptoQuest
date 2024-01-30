using System;
using CryptoQuest.Item.Equipment;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.Status.UI.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour
    {
        public event Action<UIEquipmentItem> EquipItem;
        [SerializeField] private UIEquipment _equipmentUI;
        public IEquipment Equipment => _equipmentUI.Equipment;
        private bool _canClick;

        public void Init(IEquipment equipment)
        {
            _canClick = true;
            _equipmentUI.Init(equipment);
            _equipmentUI.InitStone(equipment);
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
    }
}