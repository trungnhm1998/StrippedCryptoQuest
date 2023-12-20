using System;
using CryptoQuest.BlackSmith.Commons.UI;
using CryptoQuest.Menu;
using CryptoQuest.UI.Tooltips;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class EvolvableBeast
    {
        public Beast.Beast Beast { get; set; }
        public ICurrencyValueEnough GoldCheck { get; set; }
        public ICurrencyValueEnough DiamondCheck { get; set; }
    }

    public class UIBeastEvolve : MonoBehaviour
    {
        public event Action<UIBeastEvolve> OnBeastSelected;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private UIStars _uiStars;
        [SerializeField] private GameObject _baseObject;
        [SerializeField] private GameObject _materialObject;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private UICurrencyValueEnoughText _goldText;
        [SerializeField] private UICurrencyValueEnoughText _diamondText;
        public Beast.Beast Beast { get; private set; }
        public bool IsEnoughCurrencies { get; private set; }

        public void ConfigureCell(Beast.Beast beast)
        {
            Beast = beast;
            _displayName.StringReference = beast.LocalizedName;
            _icon.sprite = beast.Elemental.Icon;
            _uiStars.SetStars(beast.Stars);
        }

        public void SetupCurrencyValue(EvolvableBeast beast)
        {
            IsEnoughCurrencies = beast.GoldCheck.IsEnough && beast.DiamondCheck.IsEnough;
            _goldText.SetValueAndCheckIsEnough(beast.GoldCheck);
            _diamondText.SetValueAndCheckIsEnough(beast.DiamondCheck);
        }

        private void SetBaseObjectSelected(bool value)
        {
            _baseObject.SetActive(value);
            _background.gameObject.SetActive(value);
            _background.color = value ? _selectedColor : _defaultColor;
            _button.interactable = false;
        }

        public void SetMaterialObjectSelected(bool value)
        {
            _materialObject.SetActive(value);
            _background.gameObject.SetActive(value);
            _background.color = value ? _selectedColor : _defaultColor;
        }

        private void OnEnable()
        {
            _button.Selected += OnInspecting;
            _button.DeSelected += OnDeselected;
            _button.onClick.AddListener(SelectedBeast);
        }

        private void OnDisable()
        {
            _button.Selected -= OnInspecting;
            _button.DeSelected -= OnDeselected;
            _button.onClick.RemoveListener(SelectedBeast);
        }

        private void OnInspecting()
        {
            OnBeastSelected?.Invoke(this);
            _background.gameObject.SetActive(true);
        }

        private void OnDeselected()
        {
            _background.gameObject.SetActive(false);
        }

        private void SelectedBeast()
        {
            OnBeastSelected?.Invoke(this);
        }

        public void SetBaseMaterial()
        {
            SetBaseObjectSelected(true);
        }
    }
}