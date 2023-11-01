using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using CryptoQuest.Menu;
using CryptoQuest.ChangeClass.Interfaces;

namespace CryptoQuest.ChangeClass.View
{
    public class UICharacter : MonoBehaviour
    {
        public event Action<UICharacter> OnSubmit;
        public event Action<UICharacter> OnItemSelected;
        [SerializeField] private TextMeshProUGUI _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private MultiInputButton _button;
        private bool _isSelected = false;

        public ICharacterModel Class { get; private set; }
        
        public void ConfigureCell(ICharacterModel characterClass)
        {
            Class = characterClass;
            _displayName.text = characterClass.Name;
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
            if(_isSelected) return;
            OnItemSelected?.Invoke(this);
            _selectedBackground.SetActive(true);
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