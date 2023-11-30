using CryptoQuest.Item;
using CryptoQuest.Item.Consumable;
using CryptoQuest.Shop.UI.Panels.PreviewCharacter;
using CryptoQuest.UI.Menu;
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
        public int Price => 0;
        public int SellPrice => 0;
        public bool HasGem => false;

        private ConsumableInfo _consumable;

        public ConsumableItem(ConsumableInfo consumableInfo)
        {
            _consumable = consumableInfo;
        }

        public bool TryToBuy(IShopInventoryController controller) => controller.TryToBuy(_consumable);
        public bool TryToSell(IShopInventoryController controller) => controller.TryToSell(_consumable);
        public void PreviewItem(IPreviewItem preview) => preview.Preview(_consumable);
        public void PreviewStat(IPreviewCharacter preview) { }
    }
}
