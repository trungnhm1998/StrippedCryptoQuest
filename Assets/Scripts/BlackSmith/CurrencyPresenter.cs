using CryptoQuest.Events.UI;
using UnityEngine;

namespace CryptoQuest.BlackSmith
{
    public class CurrencyPresenter : MonoBehaviour
    {
        [SerializeField] private ShowWalletEventChannelSO _showWalletEventChannelSO;

        public void Show()
        {
            _showWalletEventChannelSO.EnableGold().Show();
        }

        public void Hide()
        {
            _showWalletEventChannelSO.Hide();
        }
    }
}