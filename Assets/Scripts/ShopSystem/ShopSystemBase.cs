using CryptoQuest.Merchant;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public abstract class ShopSystemBase : MerchantSystemBase
    {
        [SerializeField] private GameObject _selectActionPanel;

        protected override void OnInit()
        {
            _selectActionPanel.gameObject.SetActive(true);
        }

        protected override void OnExit()
        {
            _selectActionPanel.gameObject.SetActive(false);
        }
    }
}