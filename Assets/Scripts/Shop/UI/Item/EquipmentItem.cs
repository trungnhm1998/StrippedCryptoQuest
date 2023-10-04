using CryptoQuest.Gameplay.Inventory.Currency;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item;
using CryptoQuest.Item.Equipment;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Localization;

namespace CryptoQuest.Shop.UI.Item
{
    public class EquipmentItem : IShopItem
    {
        public ItemInfo Item => _equipment;
        public AssetReferenceT<Sprite> Icon => _equipment.Data.Image;
        public LocalizedString DisplayName => _equipment.Data.DisplayName;
        public int Price => _equipment.Price;
        public int SellPrice => _equipment.SellPrice;
        public bool HasGem => true;

        private EquipmentInfo _equipment;

        public EquipmentItem(EquipmentInfo equipmentInfo)
        {
            _equipment = equipmentInfo;
        }

        public bool TryToBuy(IShopInventoryController controller) => controller.TryToBuy(_equipment);
        public bool TryToSell(IShopInventoryController controller) => controller.TryToSell(_equipment);
    }
}