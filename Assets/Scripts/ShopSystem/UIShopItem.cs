using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIShopItem : MonoBehaviour
    {
        [SerializeField] private Text _txtPrice;
        private float _price;
        public float Price => _price;
        public string PriceText => _txtPrice.text;

        public void SetPrice(float price)
        {
            _price = price;
            _txtPrice.text = $"{price}G";
        }

        public abstract void OnPressed();
    }
}