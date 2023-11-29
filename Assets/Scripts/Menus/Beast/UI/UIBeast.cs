using System;
using CryptoQuest.Character.Beast;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Menus.Beast.UI
{
    public class UIBeast : MonoBehaviour
    {
        public delegate void UIBeastEvent(BeastDef beastDef);

        public static event UIBeastEvent Inspecting;
        public static event Action<UIBeast> InspectingBeastEvent;

        [SerializeField] private Image _beastIcon;
        [SerializeField] private MultiInputButton _beastButton;
        [SerializeField] private LocalizeStringEvent _beastName;
        [SerializeField] private TMP_Text _beastNameText;
        [SerializeField] private Color _disableColor;

        private Color _normalColor;
        private BeastDef _beastDef;

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

        public void Init(BeastDef beastDef)
        {
            _beastDef = beastDef;
            _beastNameText.text = _beastDef.Data.BeastTypeSo.BeastInformation.LocalizedName.GetLocalizedString();
        }

        public void OnPressButton() { }

        private void OnSelected()
        {
            Inspecting?.Invoke(_beastDef);
            InspectingBeastEvent?.Invoke(this);
        }

        private void SetDisable(bool value)
        {
            _beastNameText.color = value ? _disableColor : _normalColor;
        }
    }
}