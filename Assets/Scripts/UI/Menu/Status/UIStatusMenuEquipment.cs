using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Input;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Status
{
    public class UIStatusMenuEquipment : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private InputMediatorSO _inputMediator;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO _confirmSelectEquipmentSlotEvent;
        [SerializeField] private VoidEventChannelSO _turnOffInventoryEvent;

        [Header("Game Components")]
        [SerializeField] private List<UIStatusMenuEquipmentSlot> _equipmentSlots;
        [SerializeField] private GameObject _navigations;

        private int _currentIndex;
        private int CurrentIndex
        {
            get => _currentIndex;
            set
            {
                int count = _equipmentSlots.Count;
                _currentIndex = (value + count) % count;
            }
        }

        private UIStatusMenuEquipmentSlot _selectedSlotHolder;

        private void OnEnable()
        {
            _inputMediator.EnableChangeEquipmentModeEvent += ChangeEquipmentModeEnabled;
            _turnOffInventoryEvent.EventRaised += RegisterChangeEquipmentInputEvents;
        }

        private void OnDisable()
        {
            _inputMediator.EnableChangeEquipmentModeEvent -= ChangeEquipmentModeEnabled;
            _turnOffInventoryEvent.EventRaised -= RegisterChangeEquipmentInputEvents;
        }

        private void ChangeEquipmentModeEnabled()
        {
            _inputMediator.EnableStatusEquipmentsInput();
            Init();
        }

        private void Init()
        {
            RegisterChangeEquipmentInputEvents();
            CurrentIndex = 0;
            _selectedSlotHolder = GetEquipmentSlot(CurrentIndex);
            SelectEquipmentSlot();
        }

        private void SelectEquipmentSlot()
        {
            var currentSlot = GetEquipmentSlot(CurrentIndex);

            if (_selectedSlotHolder != currentSlot)
                _selectedSlotHolder.Deselect();

            _selectedSlotHolder = currentSlot;
            _selectedSlotHolder.Select();
        }

        private UIStatusMenuEquipmentSlot GetEquipmentSlot(int index)
        {
            return _equipmentSlots[index];
        }

        private void StatusEquipmentGoToBelowSlot()
        {
            CurrentIndex++;
            SelectEquipmentSlot();
        }
        
        private void StatusEquipmentGoToAboveSlot()
        {
            CurrentIndex--;
            SelectEquipmentSlot();
        }

        private void OnStatusMenuConfirmSelect()
        {
            _confirmSelectEquipmentSlotEvent.RaiseEvent();
            _navigations.SetActive(false);
            UnregisterChangeEquipmentInputEvents();
        }

        private void CancelEquipment()
        {
            _selectedSlotHolder.Deselect();
            UnregisterChangeEquipmentInputEvents();
            _inputMediator.EnableStatusMenuInput();
        }

        private void RegisterChangeEquipmentInputEvents()
        {
            _inputMediator.StatusEquipmentGoBelowEvent += StatusEquipmentGoToBelowSlot;
            _inputMediator.StatusEquipmentGoAboveEvent += StatusEquipmentGoToAboveSlot;
            _inputMediator.StatusMenuConfirmSelectEvent += OnStatusMenuConfirmSelect;
            _inputMediator.StatusEquipmentCancelEvent += CancelEquipment;
        }

        private void UnregisterChangeEquipmentInputEvents()
        {
            _inputMediator.StatusEquipmentGoBelowEvent -= StatusEquipmentGoToBelowSlot;
            _inputMediator.StatusEquipmentGoAboveEvent -= StatusEquipmentGoToAboveSlot;
            _inputMediator.StatusMenuConfirmSelectEvent -= OnStatusMenuConfirmSelect;
            _inputMediator.StatusEquipmentCancelEvent -= CancelEquipment;
        }
    }
}
