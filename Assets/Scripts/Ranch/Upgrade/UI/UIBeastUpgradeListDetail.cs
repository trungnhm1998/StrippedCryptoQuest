using System;
using CryptoQuest.AbilitySystem.Attributes;
using CryptoQuest.Beast;
using CryptoQuest.Beast.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace CryptoQuest.Ranch.Upgrade.UI
{
    public class UIBeastUpgradeListDetail : MonoBehaviour, ISelectHandler, IDeselectHandler
    {
        public event Action<UIBeastUpgradeListDetail> OnSubmit;
        public event Action<UIBeastUpgradeListDetail> OnInspectingBeast;

        [SerializeField] private Image _icon;
        [SerializeField] private Image _background;
        [SerializeField] private LocalizeStringEvent _displayName;
        [SerializeField] private CalculatorBeastStatsSO _calculatorBeastStatsSo;
        [SerializeField] private GameObject _selectedTag;

        public IBeast Beast = NullBeast.Instance;

        public void Init(IBeast beast)
        {
            Beast = beast;
            _selectedTag.SetActive(false);
            _background.gameObject.SetActive(false);
            SetName(beast.LocalizedName);
            SetIcon(beast.Elemental);
        }

        public void OnSelect(BaseEventData eventData)
        {
            _background.gameObject.SetActive(true);
            _calculatorBeastStatsSo.RaiseEvent(Beast);
            OnInspectingBeast?.Invoke(this);
        }

        public void OnDeselect(BaseEventData eventData)
        {
            _background.gameObject.SetActive(false);
        }

        public void SelectedBeast()
        {
            _selectedTag.SetActive(true);
            OnSubmit?.Invoke(this);
        }

        #region Setup

        private void SetName(LocalizedString value)
        {
            _displayName.StringReference = value;
        }

        private void SetIcon(Elemental beastsElemental)
        {
            _icon.sprite = beastsElemental.Icon;
        }

        #endregion
    }
}