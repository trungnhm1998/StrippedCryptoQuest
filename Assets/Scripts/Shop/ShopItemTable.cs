using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop.UI.Panels.Item;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop
{
    [Serializable]
    public abstract class ShopItemTable : ScriptableObject
    {
        public int Id;
        public List<string> Items = new ();

        public abstract IEnumerator LoadItem(Action<IShopItemData> callback);
    }
}
