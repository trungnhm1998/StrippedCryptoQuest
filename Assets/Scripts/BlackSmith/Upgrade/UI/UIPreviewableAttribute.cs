using UnityEngine;
using CryptoQuest.UI.Tooltips;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace CryptoQuest.BlackSmith.Upgrade.UI
{
    public class UIPreviewableAttribute : UIAttribute
    {
        [SerializeField] private Color _increaseColor;
        [SerializeField] private Color _decreaseColor;
        [SerializeField] private GameObject _increaseMark;
        [SerializeField] private GameObject _decreaseMark;

        public string AttributeName => _attributeShortName.text;
        public LocalizedString LocalizeAttributeName { get; private set; }

        private Color _originColor;
        private float _currentValue;

        private void Start()
        {
            _originColor = _attributeShortName.color;
        }

        public override void SetAttribute(LocalizedString attributeName, float value)
        {
            base.SetAttribute(attributeName, value);
            LocalizeAttributeName = attributeName;
            _currentValue = value;
        }

        public void SetPreviewValue(float newValue)
        {
            if (Mathf.Approximately(newValue,_currentValue)) 
                ResetPreviewUI();
            SetPreviewUI(newValue > _currentValue);
            _attributeValue.text = string.Format(_attributeValueText, newValue);
        }

        private void SetPreviewUI(bool isIncrease)
        {
            _attributeShortName.color = isIncrease ? _increaseColor : _decreaseColor;
            _attributeValue.color = isIncrease ? _increaseColor : _decreaseColor;
            _increaseMark.SetActive(isIncrease);
            _decreaseMark.SetActive(!isIncrease);
        }

        public void ResetPreviewUI()
        {
            _attributeShortName.color = _originColor;
            _attributeValue.color = _originColor;
            _increaseMark.SetActive(false);
            _decreaseMark.SetActive(false);
            _attributeValue.text = string.Format(_attributeValueText, _currentValue);
        }
    }
}