using System;
using CryptoQuest.ChangeClass.ScriptableObjects;
using CryptoQuest.Menu;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.ChangeClass.View
{
    public class UIOccupation : MonoBehaviour
    {
        public event Action<UIOccupation> OnSubmit;
        public event Action<UIOccupation> OnItemSelected;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private Image _icon;
        [SerializeField] private GameObject _selectedBackground;
        [SerializeField] private GameObject _defaultBackground;
        [SerializeField] private MultiInputButton _button;

        public ChangeClassSO Class { get; private set; }

        public void ConfigureCell(ChangeClassSO changeClass)
        {
            Class = changeClass;
            _displayName.StringReference = changeClass.CharacterClass.Name;
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
