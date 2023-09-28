using CryptoQuest.Item;
using CryptoQuest.Shop;
using PolyAndCode.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopUsable : UIShop
    {
        protected override void InitItem()
        {
            var listItem = _inventorySO.GetConsumables();

            for (int i = 0; i < listItem.Count; i++)
            {
                IShopItemData shopItemData = new ConsumableItem(listItem[i]);

                InstantiateItem(shopItemData, false);
            }
        }
    }
}
