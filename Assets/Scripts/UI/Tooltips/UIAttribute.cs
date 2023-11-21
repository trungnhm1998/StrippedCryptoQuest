using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public class UIAttribute : MonoBehaviour
    {
        [SerializeField] private Text _attributeShortName;
        [SerializeField] private Text _attributeValue;

        private static string _attributeValueText;

        private void Awake()
        {
            _attributeValueText = _attributeValue.text;
        }

        public void SetAttribute(LocalizedString attributeName, float value)
        {
            attributeName.GetLocalizedStringAsync().Completed += (handle) => _attributeShortName.text = handle.Result;
            _attributeValue.text = string.Format(_attributeValueText, value);
        }
    }
}