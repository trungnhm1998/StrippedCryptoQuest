using CryptoQuest.BlackSmith.Interface;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Evolve.UI
{
    public class UIEquipmentItem : MonoBehaviour, ISelectHandler
    {
        public event UnityAction<UIEquipmentItem> InspectingEquipmentEvent;
        public event UnityAction<UIEquipmentItem> SelectedEquipmentEvent;
        public event UnityAction<UIEquipmentItem> SelectedEquipmentAsMaterialEvent;

        [SerializeField] private Image _icon;
        [SerializeField] private LocalizeStringEvent _nameLocalize;
        [SerializeField] private GameObject _selectedTag;

        private IEvolvableEquipment _equipmentData;
        public IEvolvableEquipment EquipmentData { get => _equipmentData; }

        public void SetItemData(IEvolvableEquipment equipment)
        {
            ResetItemStates();

            _icon.sprite = equipment.Icon;
            _nameLocalize.StringReference = equipment.LocalizedName;

            _equipmentData = equipment;
        }

        public void ResetItemStates()
        {
            _selectedTag.SetActive(false);
        }

        public void OnSelect(BaseEventData eventData)
        {
            // temp solution for the asynchronous issue, will refactor later
            Invoke(nameof(InspectEquipment), .1f);
        }

        private void InspectEquipment()
        {
            InspectingEquipmentEvent?.Invoke(this);
        }

        public void SubmitEquipment()
        {
            SelectedEquipmentEvent?.Invoke(this);
        }

        public void SetAsBase()
        {
            _selectedTag.SetActive(true);
        }
    }
}
