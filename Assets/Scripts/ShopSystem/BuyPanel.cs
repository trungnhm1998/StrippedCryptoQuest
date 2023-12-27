using CryptoQuest.Merchant;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.ShopSystem
{
    public class BuyPanel : MonoBehaviour
    {
        [SerializeField] private UnityEvent _closing;
        [SerializeField] private MerchantInput _input;

        private void OnEnable()
        {
            _input.CancelEvent += OnCancel;
        }

        private void OnDisable()
        {
            _input.CancelEvent -= OnCancel;
        }

        private bool _selectedItemToPurchase;

        private void OnCancel()
        {
            if (!_selectedItemToPurchase)
            {
                _closing.Invoke();
                return;
            }
        }
    }
}