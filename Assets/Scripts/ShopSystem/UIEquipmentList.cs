using CryptoQuest.Gameplay.Inventory.ScriptableObjects;
using CryptoQuest.Item.Equipment;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public abstract class UIEquipmentList : UIInventoryItemList<UIEquipmentShopItem>
    {
        [SerializeField] private InventorySO _inventory;

        protected override void Render()
        {
            foreach (var item in _inventory.Equipments)
            {
                if (item.IsNft || IsIgnoreType(item)) continue;
                var uiItem = GetItem(PriceMappingDatabase.GetPrice(item.Prefab.ID));
                uiItem.Render(item);
            }
        }

        protected abstract bool IsIgnoreType(IEquipment equipment);
    }
}