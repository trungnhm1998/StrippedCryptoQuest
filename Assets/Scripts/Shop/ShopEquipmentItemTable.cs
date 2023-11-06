using CryptoQuest.Gameplay.Inventory;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop.UI.Item;
using CryptoQuest.System;
using System;
using System.Collections;
using UnityEngine;

namespace CryptoQuest.Shop
{
    [CreateAssetMenu(fileName = "New shop table", menuName = "Crypto Quest/Shop/New equipment Table")]
    public class ShopEquipmentItemTable : ShopItemTable
    {
        public override IEnumerator LoadItem(Action<IShopItem> callback)
        {
            // var consumableProvider = ServiceProvider.GetService<IEquipmentDefProvider>();
            // for (int i = 0; i < Items.Count; i++)
            // {
            //     var equip = new EquipmentInfo(Items[i]);
            //
            //     yield return consumableProvider.Load(equip);
            //
            //     IShopItem shopItemData = new EquipmentItem(equip);
            //
            //     callback.Invoke(shopItemData);
            // }
            yield break;
        }
    }
}
