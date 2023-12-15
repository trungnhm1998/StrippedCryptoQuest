using UnityEngine;

namespace CryptoQuest.BlackSmith.Commons.UI
{
    public interface ICurrencyValueEnough
    {
        float Value { get; }
        bool IsEnough { get; }
    }

    public class CurrencyValueEnough : ICurrencyValueEnough
    {
        public float Value { get; set; }
        public bool IsEnough { get; set; }
    }

    public class UICurrencyValueEnoughText : UICurrencyText
    {
        [SerializeField] private Color _enoughGoldColor;
        [SerializeField] private Color _notEnoughGoldColor;

        public void SetValueAndCheckIsEnough(ICurrencyValueEnough currencyValue)
        {
            SetValue(currencyValue.Value);
            SetColor(currencyValue.IsEnough);
        }

        private void SetColor(bool isEnough)
        {
            _currencyText.color = isEnough ? _enoughGoldColor : _notEnoughGoldColor;
        }
    }
}
