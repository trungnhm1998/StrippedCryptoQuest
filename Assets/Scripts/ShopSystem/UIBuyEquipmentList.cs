using System;
using CryptoQuest.Item.Equipment;
using CryptoQuest.UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace CryptoQuest.ShopSystem
{
    public class UIBuyEquipmentList : MonoBehaviour
    {
        public event Action<UIEquipmentShopItem> BuyingEquipment;

        [SerializeField] private SelectFirstChildInList _selectFirstChildInList;
        [SerializeField] private PriceMappingDatabase _priceMappingDatabase;
        [SerializeField] private UIShopItemPool<UIEquipmentShopItem> _pool;
        [SerializeField] private ScrollRect _scrollRect;
        private void OnDisable() => Clear();

        public void Render(IEquipment[] sellingItems)
        {
            Clear();
            foreach (var sellingItem in sellingItems)
            {
                if (_priceMappingDatabase.TryGetBuyingPrice(sellingItem.Data.ID, out var price) == false) continue;
                var uiItem = GetItem(price);
                uiItem.Render(sellingItem);
            }

            _selectFirstChildInList.Select();
        }

        private UIEquipmentShopItem GetItem(float price)
        {
            var uiShopItem = _pool.Get(price);
            uiShopItem.transform.parent = _scrollRect.content;
            uiShopItem.Pressed += OnBuyingItem;
            return uiShopItem;
        }

        private void Clear()
        {
            foreach (var child in _scrollRect.content.GetComponentsInChildren<UIEquipmentShopItem>())
                Release(child);
        }

        public void Release(UIEquipmentShopItem item)
        {
            item.Pressed -= OnBuyingItem;
            _pool.Release(item);
        }

        private void OnBuyingItem(UIEquipmentShopItem item)
        {
            BuyingEquipment?.Invoke(item);
        }
    }
}