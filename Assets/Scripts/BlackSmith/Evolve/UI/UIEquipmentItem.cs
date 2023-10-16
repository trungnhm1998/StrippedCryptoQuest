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

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private GameObject _selectedTag;

        private bool _isSelected = false;

        private IEvolvableData _equipmentData;
        public IEvolvableData EquipmentData { get => _equipmentData; }

        public void SetItemData(IEvolvableData equipment)
        {
            _icon.sprite = equipment.Icon;
            _nameLocalize.StringReference = equipment.LocalizedName;

            _equipmentData = equipment;
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
            _isSelected = !_isSelected;
            _selectedTag.SetActive(_isSelected);
            SelectedEquipmentEvent?.Invoke(this);
        }
    }
}