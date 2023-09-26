using CryptoQuest.Shop.UI.Panels.Menu;
using UnityEngine;

namespace CryptoQuest.Shop.UI.ShopStates.Menu
{
    public class ShopMenuState : ShopStateBase
    {
        protected UIMenuPanel MenuPanel { get; }
        public ShopMenuState(UIMenuPanel panel) : base(panel)
        {
            MenuPanel = panel;
        }

        public override void HandleBack()
        {
            base.HandleBack();
            _shopContext.HideShop();
        }
    }
}
