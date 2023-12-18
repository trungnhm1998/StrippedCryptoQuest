using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item.Type;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumableList : UIInventoryItemList<UIConsumableShopItem>
    {
        protected override void Render()
        {
            foreach (var consumable in Inventory.Consumables)
            {
                if (consumable.Data.ConsumableType != EConsumableType.Consumable || consumable.Quantity <= 0) continue;
                var uiItem = GetItem(PriceMappingDatabase.GetPrice(consumable));
                uiItem.Render(consumable);
            }
        }
    }
}