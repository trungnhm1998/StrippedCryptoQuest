using CryptoQuest.Item.Consumable;
using UnityEngine;

namespace CryptoQuest.ShopSystem
{
    public class UIConsumableShopItem : UIShopItem
    {
        [SerializeField] private UIConsumable _uiConsumable;
        
        public void Render(ConsumableInfo consumable)
        {
            _uiConsumable.Init(consumable);
        }
    }
}