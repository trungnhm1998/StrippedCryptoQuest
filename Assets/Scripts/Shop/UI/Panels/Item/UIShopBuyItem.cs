using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopBuyItem : UIShopBuy
    {
        [SerializeField] private ConsumableDatabase _consumableDatabase;

        protected override void InitItem()
        {
            StartCoroutine(LoadItem(_shopItemTable.Items));
        }

        IEnumerator LoadItem(List<string> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                yield return _consumableDatabase.LoadDataById(items[i]);
                var shopItem = _consumableDatabase.GetDataById(items[i]);
                if (shopItem != null)
                {
                    IShopItemData shopItemData = new ConsumableItem(new ConsumableInfo(shopItem));

                    InstantiateItem(shopItemData, true, i);
                }
            }

            yield return SelectDefaultButton();
        }
    }
}
