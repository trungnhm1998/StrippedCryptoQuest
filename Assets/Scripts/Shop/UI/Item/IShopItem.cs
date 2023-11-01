using CryptoQuest.Item;
using CryptoQuest.Shop.UI.Panels.PreviewCharacter;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Item
{
    public interface IShopItem
    {
        public ItemInfo Item { get; }
        public AssetReferenceT<Sprite> Icon { get; }
        public LocalizedString DisplayName { get; }
        public int Price { get; }
        public int SellPrice { get; }
        public bool HasGem { get; }

        public bool TryToBuy(IShopInventoryController controller);
        public bool TryToSell(IShopInventoryController controller);
        public void PreviewItem(IPreviewItem uishop);
        public void PreviewStat(IPreviewCharacter previewStat);
    }
}