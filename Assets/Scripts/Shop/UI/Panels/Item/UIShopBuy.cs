using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;
using CryptoQuest.Shop;
using CryptoQuest.System;
using CryptoQuest.Shop.UI.ScriptableObjects;
using PolyAndCode.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopBuy : UIShop
    {
        protected ShopItemTable _shopItemTable;

        public override void Show()
        {
            _content.SetActive(true);
        }

        public void SetItemShopTable(ShopItemTable shopItemTable)
        {
            _shopItemTable = shopItemTable;
            ResetShopItem();
            InitItem();
        }

        private IEnumerator LoadItemTable(ShopItemTable shopItemTable)
        {
            yield return shopItemTable.LoadItem(CreateItem);
            yield return SelectDefaultButton();
        }

        private void CreateItem(IShopItemData shopItemData)
        {
            InstantiateItem(shopItemData, true);
        }    

        protected override void InitItem()
        {
            StartCoroutine(LoadItemTable(_shopItemTable));
        }
    }
}
