using System;
using TMPro;
using UnityEngine;
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

        public ICharacterModel Class { get; private set; }

        public void ConfigureCell(ICharacterModel characterClass)
        {
            Class = characterClass;
            _displayName.text = characterClass.Name;
        }

        private void OnEnable()
        {
            _button.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _button.Selected -= OnSelected;
        }

        private void OnSelected()
        {
            OnItemSelected?.Invoke(this);
        }

        public void EnableButtonBackground(bool isEnable)
        {
            _selectedBackground.SetActive(isEnable);
        }
    }
}