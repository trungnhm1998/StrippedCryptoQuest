using CryptoQuest.Shop.UI.Item;

namespace CryptoQuest.Shop.UI.Panels.Item
{
    public class UIShopArmor : UIShop
    {

        protected override void InitItem()
        {
            var listItem = _inventorySO.GetNonWeapons();

            for (int i = 0; i < listItem.Count; i++)
            {
                IShopItem shopItemData = new EquipmentItem(listItem[i]);

                InstantiateItem(shopItemData, false);
            }
        }
    }
}
