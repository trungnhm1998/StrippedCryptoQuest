using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.UI.Tooltips
{
    public class UIAttribute : MonoBehaviour
    {
        [SerializeField] protected Text _attributeShortName;
        [SerializeField] protected Text _attributeValue;

        protected static string _attributeValueText;

        private void Awake()
        {
            _attributeValueText = _attributeValue.text;
        }

        public virtual void SetAttribute(LocalizedString attributeName, float value)
        {
            attributeName.GetLocalizedStringAsync().Completed += (handle) => _attributeShortName.text = handle.Result;
            _attributeValue.text = string.Format(_attributeValueText, value);
        }
    }
}