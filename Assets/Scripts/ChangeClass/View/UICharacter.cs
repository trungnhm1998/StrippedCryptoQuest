using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using CryptoQuest.Menu;
using CryptoQuest.ChangeClass.API;

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

        public CharacterAPI Class { get; private set; }

        public void ConfigureCell(CharacterAPI characterClass)
        {
            Class = characterClass;
            _displayName.text = characterClass.name;
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