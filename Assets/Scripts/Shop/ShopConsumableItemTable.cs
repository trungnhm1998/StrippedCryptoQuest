using CryptoQuest.Item;
using CryptoQuest.Shop.UI.Item;
using CryptoQuest.Shop.UI.Panels.Item;
using CryptoQuest.System;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.Shop
{
    [CreateAssetMenu(fileName = "New shop table", menuName = "Crypto Quest/Shop/New Consumable Table")]
    public class ShopConsumableItemTable : ShopItemTable
    {
        public override IEnumerator LoadItem(Action<IShopItem> callback)
        {
            var consumableProvider = ServiceProvider.GetService<IConsumableProvider>();
            for (int i = 0; i < Items.Count; i++)
            {
                var consumable = new ConsumableInfo(Items[i]);

                yield return consumableProvider.Load(consumable);

                IShopItem shopItemData = new ConsumableItem(consumable);

                callback.Invoke(shopItemData);
            }
        }
    }

}