using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.UI.Equipment;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class UIEquipmentShopItem : UIShopItem
    {
        [SerializeField] private UIEquipment _uiEquipment;
        
        public void Render(IEquipment item)
        {
            _uiEquipment.Init(item);
        }
    }
}