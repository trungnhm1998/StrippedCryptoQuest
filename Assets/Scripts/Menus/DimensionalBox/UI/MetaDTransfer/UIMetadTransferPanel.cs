using System;
using CryptoQuest.Core;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
using CryptoQuest.UI.Actions;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class UIMetadTransferPanel : MonoBehaviour
    {
        public event Action TransferSourceChanged;
        [SerializeField] private WalletSO _wallet;
        public CurrencySO SourceToTransfer { get; private set; }
        [field: SerializeField] public InputMediatorSO Input { get; private set; }
        [field: SerializeField] public UIMetadSourceButton GameButton { get; private set; }
        [field: SerializeField] public UIMetadSourceButton DimensionalBoxButton { get; private set; }
        [field: SerializeField] public InputField TransferAmountInput { get; private set; }
        public bool Transferring { get; set; }

        private void OnEnable()
        {
            if (Transferring) ActionDispatcher.Dispatch(new ShowLoading());
            Invoke(nameof(SelectDefaultButton), 0);
        }

        private void OnDisable() => ActionDispatcher.Dispatch(new ShowLoading(false));

        public void SelectDefaultButton() => EventSystem.current.SetSelectedGameObject(GameButton.gameObject);

        public void SelectGameButton()
        {
            if (Transferring) return;
            SourceToTransfer = GameButton.GetComponentInChildren<UICurrency>().Currency;
            TransferSourceChanged?.Invoke();
        }

        public void SelectDimensionalBoxButton()
        {
            if (Transferring) return;
            SourceToTransfer = DimensionalBoxButton.GetComponentInChildren<UICurrency>().Currency;
            TransferSourceChanged?.Invoke();
        }
    }
}