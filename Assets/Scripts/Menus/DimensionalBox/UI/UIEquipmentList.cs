using System;
using System.Collections.Generic;
using CryptoQuest.Core;
using CryptoQuest.Events;
using CryptoQuest.Menus.DimensionalBox.Objects;
using CryptoQuest.Menus.DimensionalBox.States;
using CryptoQuest.Sagas.Objects;
using TinyMessenger;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI
{
    public class UIEquipmentList : MonoBehaviour
    {
        public event Action<UIEquipment> Transferring;
        public event Action<UIEquipmentList> Initialized;

        [SerializeField] private bool _wrapAround = true;
        [SerializeField] private ScrollRect _scrollView;
        [SerializeField] private EquipmentUIPool _pool;

        [Header("Listen to")]
        [SerializeField] private GetEquipmentsEvent _getEquipmentsEvent;

        [Header("Raise on")]
        [SerializeField] private StringEventChannelSO _transferEquipmentEvent;

        private List<UIEquipment> _equipmentsToTransfer = new();
        public bool PendingTransfer => _equipmentsToTransfer.Count > 0;
        private TinyMessageSubscriptionToken _confirmTransferEvent;

        private void OnEnable()
        {
            _confirmTransferEvent = ActionDispatcher.Bind<ConfirmTransferAction>(OnTransferEquipment);
            _getEquipmentsEvent.EventRaised += Initialize;
            
            Initialized?.Invoke(this);
        }

        private void OnDisable()
        {
            ActionDispatcher.Unbind(_confirmTransferEvent);
            _getEquipmentsEvent.EventRaised -= Initialize;
        }

        private List<EquipmentResponse> _equipments;
        private void Initialize(List<EquipmentResponse> equipments)
        {
            _equipments = equipments;
            ClearOldEquipments();
            foreach (var equipment in equipments)
            {
                var uiEquipment = _pool.Get();
                uiEquipment.Pressed += OnTransferring;
                uiEquipment.transform.SetParent(_scrollView.content);
                uiEquipment.Initialize(equipment);
            }

            if (equipments.Count > 0) Initialized?.Invoke(this);
        }

        public void Transfer(UIEquipment uiEquipment)
        {
            if (_equipments.Find(item => item.id == uiEquipment.Id) == null)
            {
                _equipmentsToTransfer.Add(uiEquipment);
                uiEquipment.EnablePendingTag(true);
            }

            uiEquipment.Pressed += OnTransferring;
            uiEquipment.transform.SetParent(_scrollView.content);
            uiEquipment.transform.SetAsLastSibling();
            CurrentSelectedIndex = _scrollView.content.transform.childCount - 1;
        }

        private void ClearOldEquipments()
        {
            CurrentSelectedIndex = 0;
            _equipmentsToTransfer.Clear();
            var oldEquipments = _scrollView.content.GetComponentsInChildren<UIEquipment>();
            foreach (var equipment in oldEquipments)
            {
                equipment.Pressed -= OnTransferring;
                _pool.Release(equipment);
            }
        }

        private void OnTransferring(UIEquipment ui)
        {
            if (_equipmentsToTransfer.Remove(ui)) ui.EnablePendingTag(false);
            CurrentSelectedIndex -= 1;
            ui.Pressed -= OnTransferring;
            Transferring?.Invoke(ui);
        }

        public bool Focus(bool resetToTop = false)
        {
            if (_scrollView.content.transform.childCount == 0) return false;
            CurrentSelectedIndex = resetToTop ? 0 : CurrentSelectedIndex;
            SelectChild(CurrentSelectedIndex);
            return true;
        }

        private int _currentSelectedIndex;

        private int CurrentSelectedIndex
        {
            get => _currentSelectedIndex;
            set
            {
                _currentSelectedIndex = value;
                var childCount = _scrollView.content.transform.childCount;
                if (childCount == 0)
                {
                    _currentSelectedIndex = 0;
                    return;
                }

                if (_currentSelectedIndex >= 0 &&
                    _currentSelectedIndex < childCount) return;
                _currentSelectedIndex = _wrapAround
                    ? (_currentSelectedIndex + childCount) % childCount
                    : Mathf.Clamp(_currentSelectedIndex, 0, childCount - 1);
            }
        }

        public void Navigate(float dirY)
        {
            if (dirY == 0) return;
            CurrentSelectedIndex += (int)dirY;
            SelectChild(CurrentSelectedIndex);
        }

        private void SelectChild(int selectedIndex)
        {
            var childToSelect = _scrollView.content.transform.GetChild(selectedIndex);
            if (childToSelect == null) childToSelect = _scrollView.content.transform.GetChild(0);
            EventSystem.current.SetSelectedGameObject(childToSelect.gameObject);
        }

        private void OnTransferEquipment(ConfirmTransferAction confirmTransferAction)
        {
            if (_equipmentsToTransfer.Count == 0) return;
            string equipments = "";
            for (var index = 0; index < _equipmentsToTransfer.Count; index++)
            {
                var equipment = _equipmentsToTransfer[index];
                var comma = index == _equipmentsToTransfer.Count - 1 ? "" : ",";
                equipments += $"{equipment.Id.ToString()}{comma}";
            }

            _transferEquipmentEvent.RaiseEvent(equipments);
        }
    }
}