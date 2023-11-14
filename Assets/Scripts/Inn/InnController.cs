using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Inn
{
    public class InnController : MonoBehaviour
    {
        [Header("Raise Events")]
        [SerializeField] private ShowWalletEventChannelSO _showWalletEventChannelSO;

        [Header("Wallet Settings")]
        [SerializeField] private WalletSO _wallet;

        [SerializeField] private CurrencySO _gold;

        [Header("Components")]
        [SerializeField] private RestorationController _restorationController;

        [SerializeField] private InnPresenter _innPresenter;
        private float _currentGold => _wallet[_gold].Amount;
        private float _innCost => _innPresenter.InnPrice;
        
        public void ShowWallet() => _showWalletEventChannelSO.Show();
        public void HideWallet() => _showWalletEventChannelSO.Hide();

        public void RestoreParty()
        {
            if (!_restorationController.RestoreParty())
            {
                Debug.LogWarning($"Party is not valid or not alive");
                return;
            }

            ReduceGold();
        }

        private void ReduceGold() => _wallet[_gold].SetAmount(_currentGold - _innCost);
    }
}