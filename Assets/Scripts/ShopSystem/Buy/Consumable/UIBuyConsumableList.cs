using System;
using CryptoQuest.Item.Consumable;
using CryptoQuest.UI.Common;
using CryptoQuest.UI.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem.Buy.Consumable
{
    public class UIBuyConsumableList : MonoBehaviour
    {
        public event Action<UIConsumableShopItem> BuyingConsumable;

        [SerializeField] private PriceMappingDatabase _priceMapping;
        [SerializeField] private UIShopItemPool<UIConsumableShopItem> _pool;
        [SerializeField] private ScrollRect _scrollRect;

        public void Render(ConsumableSO[] sellingItems)
        {
            Clear();
            foreach (var item in sellingItems)
            {
                if (!_priceMapping.TryGetBuyingPrice(item.ID, out var price)) continue;
                var uiItem = _pool.Get(price);
                uiItem.transform.parent = _scrollRect.content;
                uiItem.Render(new ConsumableInfo(item));
                uiItem.Pressed += OnBuyingItem;
            }

            _scrollRect.content.GetOrAddComponent<SelectFirstChildInList>().Select();
        }

        private void OnBuyingItem(UIConsumableShopItem item) => BuyingConsumable?.Invoke(item);

        private void Clear()
        {
            foreach (var child in _scrollRect.content.GetComponentsInChildren<UIConsumableShopItem>())
            {
                child.Pressed -= OnBuyingItem;
                _pool.Release(child);
            }
        }
    }
}