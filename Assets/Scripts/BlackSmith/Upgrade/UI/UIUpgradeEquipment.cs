using System;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIUpgradeEquipment : MonoBehaviour
    {
        public event Action<UIUpgradeEquipment> OnSubmit;
        public event Action<UIUpgradeEquipment> OnItemSelected;

        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _selectedBackground;
        [SerializeField] private GameObject _confirmSelectedMark;
        [SerializeField] private GameObject _costObject;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private Color _selectedColor;

        private Color _originColor;
        public MultiInputButton Button => _button;
        public IEquipment UpgradeEquipment { get; private set; }

        private void Awake()
        {
            _originColor = _selectedBackground.color;
        }

        public void SetupUI(IEquipment equipment)
        {
            _selectedBackground.gameObject.SetActive(false);
            
            UpgradeEquipment = equipment;
            _displayName.StringReference = equipment.DisplayName;
            _icon.sprite = equipment.Type.Icon;

            SetConfirmSelected(false);
        }

        public void SetConfirmSelected(bool value)
        {
            _confirmSelectedMark.SetActive(value);
            _selectedBackground.color = value ? _selectedColor : _originColor;
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
            _selectedBackground.gameObject.SetActive(true);
        }

        private void OnDeselected()
        {
            var isSelecting = _confirmSelectedMark.activeSelf;
            if (isSelecting) return;
            _selectedBackground.gameObject.SetActive(false);
        }

        private void SelectedEquipment()
        {
            OnSubmit?.Invoke(this);
            SetConfirmSelected(true);
        }
    }
}