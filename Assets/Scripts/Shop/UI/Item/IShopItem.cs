using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.UI.Menu.Panels.Status;
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
    }
}
