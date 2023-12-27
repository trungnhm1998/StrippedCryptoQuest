using CryptoQuest.Item.Consumable;
using UnityEngine;

namespace CryptoQuest.ShopSystem.Buy.Consumable
{
    public class BuyConsumableModel : MonoBehaviour
    {
        [SerializeField] private ConsumableSO[] _sellingConsumables;
        public ConsumableSO[] SellingConsumables => _sellingConsumables;
    }
}