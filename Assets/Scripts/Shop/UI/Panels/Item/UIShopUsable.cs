using CryptoQuest.Shop.UI.Item;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopUsable : UIShop
    {
        protected override void InitItem()
        {
            var listItem = _inventorySO.GetConsumables();

            for (int i = 0; i < listItem.Count; i++)
            {
                IShopItem shopItemData = new ConsumableItem(listItem[i]);

                InstantiateItem(shopItemData, false);
            }
        }
    }
}
