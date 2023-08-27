using System;
using CryptoQuest.Gameplay.Inventory.Items;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.UI.Menu.Panels.Status.Equipment
{
    // wrapper for UIEquipment
    public class UIEquipmentItem : MonoBehaviour
    {
        public event Action<EquipmentInfo> Inspecting;
        [FormerlySerializedAs("_equipment")]
        [SerializeField] private UIEquipment _equipmentUI;
        [SerializeField] private MultiInputButton _button;

        private void OnEnable()
        {
            _button.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
        }

        private void OnSelected()
        {
            if (_equipmentUI.Equipment.IsValid())
                Inspecting?.Invoke(_equipmentUI.Equipment);
        }

        public void Init(EquipmentInfo equipment)
        {
            _equipmentUI.Init(equipment);
        }
    }
}