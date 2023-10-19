using System;
using CryptoQuest.BlackSmith.Interface;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEquipmentItem : MonoBehaviour, ISelectHandler
    {
        public event UnityAction<IEvolvableData> InspectingEquipmentEvent;
        public event UnityAction<UIEquipmentItem> SelectedEquipmentEvent;
        public event UnityAction<UIEquipmentItem> SelectedEquipmentAsMaterialEvent;

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private GameObject _selectedTag;

        private bool _isTargetSelected = false;
        public bool IsTargetSelected { get => _isTargetSelected; set => _isTargetSelected = value; }

        private bool _isTarget = false;
        private bool _isMaterialSelected = false;

        private IEvolvableData _equipmentData;
        public IEvolvableData EquipmentData { get => _equipmentData; }

        public void SetItemData(IEvolvableData equipment)
        {
            ResetItemStates();

            _icon.sprite = equipment.Icon;
            _nameLocalize.StringReference = equipment.LocalizedName;

            _equipmentData = equipment;
        }

        private void ResetItemStates()
        {
            _isTarget = false;
            _isMaterialSelected = false;
            _isTargetSelected = false;
            _selectedTag.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData)
        {
            // temp solution for the asynchronous issue, will refactor later
            Invoke(nameof(InspectEquipment), .1f);
        }

        private void InspectEquipment()
        {
            InspectingEquipmentEvent?.Invoke(_equipmentData);
        }

        public void SelectToEvolve()
        {
            if (IsTargetSelected) return;
            Debug.Log($"Item Selected");
            _isTarget = true;
            _selectedTag.SetActive(true);
            SelectedEquipmentEvent?.Invoke(this);
        }

        public void SelectMaterial()
        {
            if (!_isMaterialSelected && !_isTarget)
            {
                if (IsTargetSelected)
                {
                    _isMaterialSelected = true;
                    SelectedEquipmentAsMaterialEvent?.Invoke(this);
                    Debug.Log($"Material Selected");
                }
            }
        }
    }
}