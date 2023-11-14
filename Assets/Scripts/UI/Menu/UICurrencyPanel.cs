using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.UI.Menu
{
    public enum ECurrencyType
    {
        Gold = 0,
        Diamond = 1,
        Soul = 2
    }

    public class UICurrencyPanel : MonoBehaviour
    {
        [Header("Wallet Settings")]
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _gold;
        [SerializeField] private CurrencySO _diamond;
        [SerializeField] private CurrencySO _soul;

        [Header("Listened Events")]
        [SerializeField] private ShowWalletEventChannelSO _showWalletEventChannelSO;

        [Header("Components")]
        [SerializeField] private RectTransform _currencyContainer;

        [SerializeField] private GameObject _goldContainer;
        [SerializeField] private GameObject _diamondContainer;
        [SerializeField] private GameObject _soulContainer;

        [SerializeField] private Text _goldText;
        [SerializeField] private Text _diamondText;
        [SerializeField] private Text _soulText;

        [SerializeField] private float _startPosX;
        [SerializeField] private float _endPosX;

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

        private void ShowCurrency(bool showGolds, bool showDiamonds, bool showSouls)
        {
            _currencyContainer.gameObject.SetActive(true);

            if (showGolds)
            {
                UpdateCurrency(_wallet[_gold].Amount);
            }

            if (showDiamonds)
            {
                UpdateCurrency(_wallet[_diamond].Amount, ECurrencyType.Diamond);
            }

            if (showSouls)
            {
                UpdateCurrency(_wallet[_soul].Amount, ECurrencyType.Soul);
            }

            _currencyContainer.DOLocalMoveX(_startPosX, 0.5f).SetEase(Ease.OutBack);
        }

        private void HideCurrency()
        {
            _currencyContainer.DOLocalMoveX(_endPosX, 0.5f).SetEase(Ease.InBack)
                .OnComplete((Hide));
        }

        private void UpdateCurrency(float value = 0, ECurrencyType type = ECurrencyType.Gold)
        {
            switch (type)
            {
                case ECurrencyType.Diamond:
                    _diamondContainer.SetActive(true);
                    UpdateDiamond(value);
                    break;
                case ECurrencyType.Soul:
                    _soulContainer.SetActive(true);
                    UpdateSoul(value);
                    break;
                default:
                case ECurrencyType.Gold:
                    _goldContainer.SetActive(true);
                    UpdateGold(value);
                    break;
            }
        }

        private void UpdateGold(float value)
        {
            _wallet[_gold].SetAmount(value);
            _goldText.text = $"{value}";
        }

        private void UpdateSoul(float value)
        {
            _wallet[_soul].SetAmount(value);
            _soulText.text = $"{value}";
        }

        private void UpdateDiamond(float value)
        {
            _wallet[_diamond].SetAmount(value);
            _diamondText.text = $"{value}";
        }

        private void Hide()
        {
            _currencyContainer.gameObject.SetActive(false);

            _goldContainer.SetActive(false);
            _diamondContainer.SetActive(false);
            _soulContainer.SetActive(false);
        }
    }
}