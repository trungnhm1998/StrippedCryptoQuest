using CryptoQuest.Item.Consumable;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumableShopItem : UIShopItem
    {
        public event UnityAction<UIConsumableShopItem> Releasing;
        public static event UnityAction<UIConsumableShopItem> Pressed;
        [SerializeField] private UIConsumable _uiConsumable;
        private ConsumableInfo _consumable;

        public ConsumableInfo Info => _consumable;
        
        public void Render(ConsumableInfo consumable)
        {
            _consumable = consumable;
            _uiConsumable.Init(consumable);
        }

        public override void OnPressed() => Pressed?.Invoke(this);
        public override void Release() => Releasing?.Invoke(this);
    }
}