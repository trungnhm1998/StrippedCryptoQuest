using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class MetadInputValidator : MonoBehaviour
    {
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
            // if (_isIngameWallet && quantityInput > _ingameMetad)
            //     _inputField.text = _ingameMetad.ToString();
            //
            // if (!_isIngameWallet && quantityInput > _webMetad)
            //     _inputField.text = _webMetad.ToString();
        }
    }
}