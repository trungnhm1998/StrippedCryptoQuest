using System;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using CryptoQuest.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Evolve.UI
{
    public class UIBeastEvolve : MonoBehaviour
    {
        public event Action<UIBeastEvolve> OnSubmit;
        public event Action<UIBeastEvolve> OnBeastSelected;
        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private TextMeshProUGUI _cost;
        [SerializeField] private GameObject _baseObject;
        [SerializeField] private GameObject _materialObject;
        [SerializeField] private GameObject _costObject;
        [SerializeField] private MultiInputButton _button;
        [SerializeField] private Sprite _selectedBackground;
        [SerializeField] private Sprite _defaultBackground;
        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;
        public Beast.Beast Beast { get; private set; }

        public void ConfigureCell(Beast.Beast beast)
        {
            Beast = beast;
            _displayName.StringReference = beast.LocalizedName;
            _icon.sprite = beast.Elemental.Icon;
        }

        private void SetBaseObjectSelected(bool value)
        {
            _baseObject.SetActive(value);
            _background.gameObject.SetActive(value);
            _background.sprite = value ? _selectedBackground : _defaultBackground;
            _button.interactable = false;
        }

        public void SetMaterialObjectSelected(bool value)
        {
            _materialObject.SetActive(value);
            _background.gameObject.SetActive(value);
            _background.sprite = value ? _selectedBackground : _defaultBackground;
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
            _calculatorBeastStatsSo.RaiseEvent(Beast);
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