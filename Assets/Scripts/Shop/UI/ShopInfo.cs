using CryptoQuest.Shop;
using CryptoQuest.Shop.UI.ScriptableObjects;
using System;

namespace CryptoQuest.Shop.UI
{
    [Serializable]
    public class ShopInfo
    {
        public ETypeShop Type;
        public ShopItemTable ItemTable;
    }
}
