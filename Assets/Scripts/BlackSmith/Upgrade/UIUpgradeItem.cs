using System.Collections;
using CryptoQuest.BlackSmith.Interface;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade
{
    public class UIUpgradeItem : MonoBehaviour
    {
        public static event UnityAction<UIUpgradeItem> SelectedItemEvent;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _selectedPanel;
        [SerializeField] private GameObject _costPanel;
        [SerializeField] private MultiInputButton _button;
        
        public IUpgradeEquipment Equipment;

        public void ConfigureCell(IUpgradeEquipment equipment)
        {
            Equipment = equipment;
            _displayName.StringReference = Equipment.DisplayName;
            _cost.text = Equipment.Cost.ToString();
            _icon.sprite = equipment.Icon;
        }

        private void OnEnable()
        {
            _button.Selected += OnSeleceted;
            _button.DeSelected += OnDeselected;
            _button.onClick.AddListener(SelectedEquipment);
        }

        private void OnDisable()
        {
            _button.Selected -= OnSeleceted;
            _button.DeSelected -= OnDeselected;
        }

        private void OnSeleceted()
        {
            _selectedBackground.SetActive(true);
        }

        private void OnDeselected()
        {
            _selectedBackground.SetActive(false);
        }

        private void SelectedEquipment()
        {
            SelectedItemEvent?.Invoke(this);
            _selectedPanel.SetActive(true);
            _costPanel.SetActive(false);
            _button.interactable = false;
        }
    }
}