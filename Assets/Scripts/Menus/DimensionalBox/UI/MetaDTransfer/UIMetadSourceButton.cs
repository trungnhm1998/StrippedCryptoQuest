using CryptoQuest.Menu;
using CryptoQuest.UI.Menu;
using UnityEngine;
using System;
using CryptoQuest.Gameplay.Inventory.Currency;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIMetadSourceButton : MonoBehaviour
    {
        public event Action<CurrencySO> SelectedCurrency;

        [SerializeField] private GameObject _arrow;
        [field: SerializeField] public UICurrency CurrencyUI { get; private set; }
        [field: SerializeField] public MultiInputButton Button { get; private set; }
        
        public bool Interactable 
        {
            get => Button.interactable;
            set 
            {
                Button.interactable = value;
            }
        }

        private void OnEnable()
        {
            Button.Selected += SetSelected;
            Button.DeSelected += SetDeselected;
            Button.onClick.RemoveAllListeners();
            Button.onClick.AddListener(() => SelectedCurrency?.Invoke(CurrencyUI.Currency));
        }

        private void OnDisable()
        {
            Button.Selected -= SetSelected;
            Button.DeSelected -= SetDeselected;
        }

        public void SetSelected()
        {
            _arrow.SetActive(true);
        }

        public void SetDeselected()
        {
            _arrow.SetActive(false);
        }
    }
}