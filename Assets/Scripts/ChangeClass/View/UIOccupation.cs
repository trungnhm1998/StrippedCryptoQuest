using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Menu;
using CryptoQuest.Character;

namespace CryptoQuest
{
    public class UIOccupation : MonoBehaviour
    {
        public event Action<UIOccupation> OnSubmit;
        public event Action<UIOccupation> OnItemSelected;
        [SerializeField] private TextMeshProUGUI _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _defaultBackground;
        [SerializeField] private MultiInputButton _button;

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
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
            _button.DeSelected -= OnDeselected;
        }

        private void OnSelected()
        {
            OnItemSelected?.Invoke(this);
        }

        private void OnDeselected()
        {
            _defaultBackground.SetActive(false);
        }

        public void EnableDefaultBackground(bool isEnable)
        {
            _defaultBackground.SetActive(isEnable);
        }

        public void EnableSelectedBackground(bool isEnable)
        {
            _selectedBackground.SetActive(isEnable);
        }
    }
}
