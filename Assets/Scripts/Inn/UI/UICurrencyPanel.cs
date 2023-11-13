using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.Inn.UI
{
    public class UICurrencyPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _currencyContainer;

        [SerializeField] private Text _currencyText;

        [SerializeField] private float _startPosX;
        [SerializeField] private float _endPosX;

        public void ShowCurrency()
        {
            _currencyContainer.gameObject.SetActive(true);
            _currencyContainer.DOLocalMoveX(_startPosX, 0.5f).SetEase(Ease.OutBack);
        }

        public void HideCurrency()
        {
            _currencyContainer.DOLocalMoveX(_endPosX, 0.5f).SetEase(Ease.InBack)
                .OnComplete((() => _currencyContainer.gameObject.SetActive(false)));
        }

        public void UpdateCurrency(float gold)
        {
            _currencyText.text = $"{gold}";
        }
    }
}