using CryptoQuest.Item.Consumable;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class ConsumableShop : ShopSystemBase
    {
        [SerializeField] private ConsumableSO[] _sellingItems;
    }
}