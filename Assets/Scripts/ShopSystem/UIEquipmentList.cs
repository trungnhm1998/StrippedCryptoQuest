using CryptoQuest.Item.Equipment;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIEquipmentList : UIInventoryItemList<UIEquipmentShopItem>
    {
        protected override void Render()
        {
            foreach (var item in Inventory.Equipments)
            {
                if (item.IsNft || IsIgnoreType(item)) continue;
                var uiItem = GetItem(PriceMappingDatabase.GetPrice(item.Prefab.ID));
                uiItem.Render(item);
            }
        }

        protected abstract bool IsIgnoreType(IEquipment equipment);
    }
}