using CryptoQuest.Shop;
using PolyAndCode.UI;
using System.Linq;
using UnityEngine;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using System.Collections.Generic;
using CryptoQuest.Item.Equipment;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopArmor : UIShop
    {

        protected override void InitItem()
        {
            var listItem = _inventorySO.GetNonWeapons();

            for (int i = 0; i < listItem.Count; i++)
            {
                IShopItemData shopItemData = new EquipmentItem(listItem[i]);

                InstantiateItem(shopItemData, false);
            }
        }
    }
}
