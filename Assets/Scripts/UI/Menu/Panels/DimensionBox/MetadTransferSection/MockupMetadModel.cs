using System.Collections;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.UI.Menu.Panels.DimensionBox.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Menu.Panels.DimensionBox.MetadTransferSection
{
    public class MockupMetadModel : MonoBehaviour, IMetadModel
    {
        public event UnityAction<float, float> CurrencyUpdated;
        [SerializeField] private WalletSO _wallet;
        [SerializeField] private float WebMetad;
        public float IngameMetad => _wallet.Diamond.Amount;
        public float GetIngameMetad()
        {
            return _wallet.Diamond.Amount;
        }

        public float GetWebMetad()
        {
            return WebMetad;
        }

        public void UpdateCurrency(float inputValue, bool isIngameWallet)
        {
            if (isIngameWallet)
            {
                WebMetad += inputValue;
                _wallet.Diamond.SetCurrencyAmount(_wallet.Diamond.Amount - inputValue);
            }
            else
            {
                WebMetad -= inputValue;
                _wallet.Diamond.SetCurrencyAmount(_wallet.Diamond.Amount + inputValue);
            }
            CurrencyUpdated?.Invoke(IngameMetad, WebMetad);
        }

        public IEnumerator CoLoadData()
        {
            yield return new WaitForSeconds(1);
        }
    }
}