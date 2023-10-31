using System;
using System.Collections.Generic;
using CryptoQuest.DimensionalBox.Events;
using CryptoQuest.DimensionalBox.Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.DimensionalBox.UI
{
    public class UIEquipmentList : MonoBehaviour
    {
        public event Action<UIEquipment> Transferring;
        public event Action<UIEquipmentList> Initialized;

        [SerializeField] private bool _wrapAround = true;
        [SerializeField] private GetEquipmentsEvent _getEquipmentsEvent;
        [SerializeField] private ScrollRect _scrollView;
        [SerializeField] private EquipmentUIPool _pool;


        private void OnEnable()
        {
            _getEquipmentsEvent.EventRaised += EventRaised;
        }

        private void OnDisable()
        {
            _getEquipmentsEvent.EventRaised -= EventRaised;
        }

        private void EventRaised(List<NftEquipment> equipments)
        {
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
            uiEquipment.Pressed += OnTransferring;
            uiEquipment.transform.SetParent(_scrollView.content);
            uiEquipment.transform.SetAsLastSibling();
            CurrentSelectedIndex = _scrollView.content.transform.childCount - 1;
        }

        private void ClearOldEquipments()
        {
            var oldEquipments = _scrollView.content.GetComponentsInChildren<UIEquipment>();
            foreach (var equipment in oldEquipments)
            {
                equipment.Pressed -= OnTransferring;
                _pool.Release(equipment);
            }
        }

        private void OnTransferring(UIEquipment ui)
        {
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
    }
}