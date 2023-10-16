using System;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeItem : MonoBehaviour
    {
        public event Action<UIUpgradeItem> OnSubmit;
        public event Action<UIUpgradeItem> OnItemSelected;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _selectedPanel;
        [SerializeField] private GameObject _costPanel;
        [SerializeField] private MultiInputButton _button;

        public IUpgradeEquipment UpgradeEquipment { get; private set; }

        public void ConfigureCell(IUpgradeEquipment equipment)
        {
            UpgradeEquipment = equipment;
            _displayName.StringReference = UpgradeEquipment.DisplayName;
            _cost.text = UpgradeEquipment.Cost.ToString();
            _icon.sprite = equipment.Icon;
        }

        private void OnEnable()
        {
            _button.Selected += OnSelected;
            _button.DeSelected += OnDeselected;
            _button.onClick.AddListener(SelectedEquipment);
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
            _button.DeSelected -= OnDeselected;
        }

        private void OnSelected()
        {
            OnItemSelected?.Invoke(this);
            _selectedBackground.SetActive(true);
        }

        private void OnDeselected()
        {
            _selectedBackground.SetActive(false);
        }

        private void SelectedEquipment()
        {
            OnSubmit?.Invoke(this);
            _selectedPanel.SetActive(true);
            _costPanel.SetActive(false);
            _button.interactable = false;
        }
    }
}