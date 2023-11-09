using System;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Input;
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

        private void OnEnable()
        {
            Invoke(nameof(SelectDefaultButton), 0);
        }

        public void SelectDefaultButton() => EventSystem.current.SetSelectedGameObject(GameButton.gameObject);

        public void SelectGameButton()
        {
            SourceToTransfer = GameButton.GetComponentInChildren<UICurrency>().Currency;
            TransferSourceChanged?.Invoke();
        }

        public void SelectDimensionalBoxButton()
        {
            SourceToTransfer = DimensionalBoxButton.GetComponentInChildren<UICurrency>().Currency;
            TransferSourceChanged?.Invoke();
        }
    }
}