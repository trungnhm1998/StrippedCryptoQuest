using CryptoQuest.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIEquipmentList : UIInventoryItemList<UIEquipmentShopItem>
    {
        [SerializeField] private EquipmentInventory _equipmentInventory;

        /// <summary>
        /// Only render non nft equipment and already config price
        /// </summary>
        protected override void Render()
        {
            foreach (var item in _equipmentInventory.Equipments)
            {
                if (item.IsNft || IsIgnoreType(item)) continue;
                if (PriceMappingDatabase.TryGetSellingPrice(item.Data.ID, out var price) == false) continue;
                var uiItem = GetItem(price);
                uiItem.Render(item);
            }
        }

        protected abstract bool IsIgnoreType(IEquipment equipment);
        protected override void OnRegisterEvent(UIEquipmentShopItem uiShopItem) => uiShopItem.Pressed += OnSelected;
        protected override void OnUnregisterEvent(UIEquipmentShopItem uiShopItem) => uiShopItem.Pressed -= OnSelected;
    }
}