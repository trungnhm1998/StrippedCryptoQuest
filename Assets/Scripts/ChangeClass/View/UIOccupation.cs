using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Menu;
using CryptoQuest.ChangeClass.Interfaces;
using CryptoQuest.Character;
using UnityEngine.Localization.Components;

namespace CryptoQuest
{
    public class UIOccupation : MonoBehaviour
    {
        public event Action<UIOccupation> OnSubmit;
        public event Action<UIOccupation> OnItemSelected;
        [SerializeField] private TextMeshProUGUI _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private MultiInputButton _button;
        private bool _isSelected = false;

        public CharacterClass Class { get; private set; }

        public void ConfigureCell(CharacterClass characterClass)
        {
            Class = characterClass;
            _displayName.text = characterClass.name;
        }

        private void OnEnable()
        {
            _button.Selected += OnSelected;
            _button.DeSelected += OnDeselected;
            _button.onClick.AddListener(CharacterClassPressed);
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
            _button.DeSelected -= OnDeselected;
        }

        private void OnSelected()
        {
            if (_isSelected) return;
            OnItemSelected?.Invoke(this);
        }

        private void OnDeselected()
        {
            _selectedBackground.SetActive(false);
        }

        private void CharacterClassPressed()
        {
            OnSubmit?.Invoke(this);
            _selectedBackground.SetActive(true);
            _button.interactable = false;
            _isSelected = true;
        }
    }
}