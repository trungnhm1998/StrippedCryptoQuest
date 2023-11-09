using System.Text.RegularExpressions;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox
{
    public class MetadInputValidator : MonoBehaviour
    {
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private UIMetadTransferPanel _transferPanel;
        [SerializeField] private InputField _inputField;

        public void ValidateInputMetad()
        {
            if (string.IsNullOrEmpty(_inputField.text)) return;

            var pattern = Regex.Match(_inputField.text, @"[^0-9]");
            if (_inputField.text.Substring(0, 1) == pattern.ToString())
            {
                _inputField.text = null;
                return;
            }

            float quantityInput = float.Parse(_inputField.text);
            var currencyAmount = _wallet[_transferPanel.SourceToTransfer].Amount;
            if (quantityInput > currencyAmount)
                _inputField.text = currencyAmount.ToString();
        }
    }
}