using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using CryptoQuest.Shop.UI.Panels.PreviewCharacter;
using CryptoQuest.UI.Menu;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Item
{
    public class EquipmentItem : IShopItem
    {
        public ItemInfo Item => null; // TODO: REFACTOR
        public AssetReferenceT<Sprite> Icon => new(""); // TODO: Refactor
        public LocalizedString DisplayName => new(); // TODO: Refactor
        public int Price => 0;
        public int SellPrice => 0;
        public bool HasGem => true;

        private Equipment _equipment;

        public EquipmentItem(Equipment equipment)
        {
            _equipment = equipment;
        }

        public bool TryToBuy(IShopInventoryController controller) => controller.TryToBuy(_equipment);
        public bool TryToSell(IShopInventoryController controller) => controller.TryToSell(_equipment);
        public void PreviewItem(IPreviewItem preview) => preview.Preview(_equipment);
        public void PreviewStat(IPreviewCharacter previewStat) => previewStat.Preview(_equipment);
    }
}