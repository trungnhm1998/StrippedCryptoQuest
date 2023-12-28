using System;
using CryptoQuest.Inventory.Currency;
using CryptoQuest.Inventory.ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class MetadTransferPanel : MonoBehaviour
    {
        public event Action<CurrencySO> SelectedCurrencySource;

        [SerializeField] private WalletSO _wallet;
        [field: SerializeField] public ConfirmTransferDialogController ConfirmDialog { get; private set; }
        [field: SerializeField] public UIMetadSourceButton DiamondButton { get; private set; }
        [field: SerializeField] public UIMetadSourceButton MetadButton { get; private set; }
        [field: SerializeField] public UIInputTransferAmount InputTransferUI { get; private set; }
        [field: SerializeField] public ArrowIndicatorPresenter ArrowIndicatorPresenter { get; private set; }
        [SerializeField] private float _maximumTransferable = 99999f;

        public CurrencySO SelectedCurrency { get; private set; }

        public float InputedValue => InputTransferUI.InputedValue;
        public bool IsInputValid => InputedValue > 0 && InputedValue <= _maximumTransferable
            && InputedValue <= _wallet[SelectedCurrency].Amount;

        private void OnEnable()
        {
            DiamondButton.SelectedCurrency += SetCurrencySource;
            MetadButton.SelectedCurrency += SetCurrencySource;
            InputTransferUI.ValueChanged += SetInputValid;
        }

        private void OnDisable()
        {
            DiamondButton.SelectedCurrency -= SetCurrencySource;
            MetadButton.SelectedCurrency -= SetCurrencySource;
            InputTransferUI.ValueChanged -= SetInputValid;
        }

        private void SetCurrencySource(CurrencySO currency)
        {
            SelectedCurrency = currency;
            SelectedCurrencySource?.Invoke(currency);
        }

        public void SelectDefaultButton()
            => EventSystem.current.SetSelectedGameObject(DiamondButton.Button.gameObject);

        public void SetInteractable(bool value)
        {
            SetButtonsInteractable(value);
            InputTransferUI.Interactable = value;
        }

        public void SetButtonsInteractable(bool value)
        {
            DiamondButton.Interactable = value;
            MetadButton.Interactable = value;
        }

        public void SetInputValid(string _)
        {
            InputTransferUI.SetInputValid(IsInputValid);
        }
    }
}