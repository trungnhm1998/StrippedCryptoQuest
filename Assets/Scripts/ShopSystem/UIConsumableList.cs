using CryptoQuest.Inventory;
using CryptoQuest.Inventory.ScriptableObjects.Item.Type;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumableList : UIInventoryItemList<UIConsumableShopItem>
    {
        [SerializeField] private ConsumableInventory _inventory;
        protected override void Render()
        {
            foreach (var consumable in _inventory.Items)
            {
                if (consumable.Data.Type != EConsumableType.Consumable || consumable.Quantity <= 0) continue;
                var uiItem = GetItem(PriceMappingDatabase.GetPrice(consumable));
                uiItem.Render(consumable);
            }
        }
    }
}