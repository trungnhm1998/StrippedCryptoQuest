using System;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeast : MonoBehaviour
    {
        public delegate void UIBeastEvent(Sagas.Objects.Beast beast);

        public static event UIBeastEvent Inspecting;
        public static event Action<UIBeast> InspectingBeastEvent;

        [SerializeField] private Image _beastIcon;
        [SerializeField] private MultiInputButton _beastButton;
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private TMP_Text _beastNameText;
        [SerializeField] private Color _disableColor;

        private Color _normalColor;
        private Sagas.Objects.Beast _beast;

        public bool Interactable
        {
            set => _beastButton.interactable = value;
        }

        private void Awake()
        {
            _normalColor = _beastNameText.color;
        }

        private void OnEnable()
        {
            _beastButton.Selected += OnSelected;
        }

        private void OnDisable()
        {
            _beastButton.Selected -= OnSelected;
        }

        public void Init(Sagas.Objects.Beast beast)
        {
            _beast = beast;
            _beastNameText.text = _beast.name;
        }

        public void OnPressButton() { }

        private void OnSelected()
        {
            Inspecting?.Invoke(_beast);
            InspectingBeastEvent?.Invoke(this);
        }

        private void SetDisable(bool value)
        {
            _beastNameText.color = value ? _disableColor : _normalColor;
        }
    }
}