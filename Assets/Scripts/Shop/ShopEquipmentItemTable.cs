using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop.UI.Panels.Item;
using CryptoQuest.System;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.Shop
{
    [CreateAssetMenu(fileName = "New shop table", menuName = "Crypto Quest/Shop/New equipment Table")]
    public class ShopEquipmentItemTable : ShopItemTable
    {
        public override IEnumerator LoadItem(Action<IShopItemData> callback)
        {
            var consumableProvider = ServiceProvider.GetService<IEquipmentDefProvider>();
            for (int i = 0; i < Items.Count; i++)
            {
                var equip = new EquipmentInfo(Items[i]);

                yield return consumableProvider.Load(equip);

                IShopItemData shopItemData = new EquipmentItem(equip);

                callback.Invoke(shopItemData);
            }
        }
    }
}
