using CryptoQuest.Item.Equipment;
using CryptoQuest.Menus.Status.UI.Equipment;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.ShopSystem
{
    public class UIEquipmentShopItem : UIShopItem
    {
        public event UnityAction<UIEquipmentShopItem> Releasing;
        public static event UnityAction<UIEquipmentShopItem> Pressed;
        [SerializeField] private UIEquipment _uiEquipment;
        public IEquipment Info => _uiEquipment.Equipment;
        
        public void Render(IEquipment item) => _uiEquipment.Init(item);

        public override void OnPressed() => Pressed?.Invoke(this);
        public override void Release() => Releasing?.Invoke(this);
    }
}