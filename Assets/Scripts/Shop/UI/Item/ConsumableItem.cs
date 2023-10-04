using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Item
{
    public class ConsumableItem : IShopItem
    {
        public ItemInfo Item => _consumable;
        public AssetReferenceT<Sprite> Icon => _consumable.Data.Image;
        public LocalizedString DisplayName => _consumable.Data.DisplayName;
        public int Price => _consumable.Price;
        public int SellPrice => _consumable.SellPrice;
        public bool HasGem => false;

        private ConsumableInfo _consumable;

        public ConsumableItem(ConsumableInfo consumableInfo)
        {
            _consumable = consumableInfo;
        }

        public bool TryToBuy(IShopInventoryController controller) => controller.TryToBuy(_consumable);
        public bool TryToSell(IShopInventoryController controller) => controller.TryToSell(_consumable);
        
    }
}
