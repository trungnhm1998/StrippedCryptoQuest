using CryptoQuest.Events.UI;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public class UICurrenciesPanel : MonoBehaviour
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private UICurrency _goldCurrency;
        [SerializeField] private UICurrency _soulCurrency;
        [SerializeField] private UICurrency _diamondCurrency;

        [Header("Listened Events")]
        [SerializeField] private ShowWalletEventChannelSO _showWalletEventChannelSO;

        [Header("Animation Settings")]
        [SerializeField] private float _duration = 0.5f;

        [SerializeField] private Ease _showEase = Ease.OutBack;
        [SerializeField] private Ease _hideEase = Ease.InBack;

        private void OnEnable()
        {
            _showWalletEventChannelSO.ShowEvent += ShowCurrency;
            _showWalletEventChannelSO.HideEvent += HideCurrency;
        }

        private void OnDisable()
        {
            _showWalletEventChannelSO.ShowEvent -= ShowCurrency;
            _showWalletEventChannelSO.HideEvent -= HideCurrency;
        }

        private void ShowCurrency(ShowWalletEventChannelSO.Context ctx)
        {
            _goldCurrency.gameObject.SetActive(ctx.ShowGolds);
            _soulCurrency.gameObject.SetActive(ctx.ShowSouls);
            _diamondCurrency.gameObject.SetActive(ctx.ShowDiamonds);

            _content.gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_content);
            _content.anchoredPosition = new Vector2(_content.rect.width, _content.anchoredPosition.y);
            _content.DOAnchorPosX(0, _duration).SetEase(_showEase);
        }

        private void HideCurrency()
        {
            _content.DOAnchorPosX(_content.rect.width, _duration).SetEase(_hideEase).OnComplete(() =>
            {
                _content.gameObject.SetActive(false);
            });
        }
    }
}