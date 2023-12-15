using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIShopItem : MonoBehaviour
    {
        [SerializeField] private Text _txtPrice;

        public void SetPrice(float price) => _txtPrice.text = $"{price}G";
    }
}