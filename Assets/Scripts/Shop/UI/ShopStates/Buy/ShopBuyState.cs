using CryptoQuest.Shop.UI.Panels.Buy;
using UnityEngine;

namespace CryptoQuest.Shop.UI.ShopStates.Buy
{
    public class ShopBuyState : ShopStateBase
    {
        public ShopBuyState(UIBuyPanel panel) : base (panel)
        {
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void HandleBack()
        {
            base.HandleBack();
            _shopStateMachine.RequestStateChange(ShopStateMachine.Menu);
        }
    }
}
