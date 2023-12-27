using CryptoQuest.Inventory;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumableList : UIInventoryItemList<UIConsumableShopItem>
    {
        [SerializeField] private ConsumableInventory _inventory;
        protected override void OnRegisterEvent(UIConsumableShopItem uiShopItem) => uiShopItem.Pressed += OnSelected;
        protected override void OnUnregisterEvent(UIConsumableShopItem uiShopItem) => uiShopItem.Pressed -= OnSelected;

        protected override void Render()
        {
            foreach (var consumable in _inventory.Items)
            {
                if (consumable.Data.Type != EConsumableType.Consumable || consumable.Quantity <= 0) continue;
                if (PriceMappingDatabase.TryGetSellingPrice(consumable.Data.ID, out var price) == false) continue;
                var uiItem = GetItem(price);
                uiItem.Render(consumable);
            }
        }
    }
}