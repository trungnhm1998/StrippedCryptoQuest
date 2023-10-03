using CryptoQuest.Shop.UI.Panels.Buy;
using UnityEngine;

namespace CryptoQuest.Shop.UI.ShopStates.Buy
{
    public class ShopBuyState : ShopStateBase
    {
        private UIBuyPanel _buyPanel;

        public ShopBuyState(UIBuyPanel panel) : base (panel)
        {
            _buyPanel = panel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void HandleBack()
        {
            base.HandleBack();
            if(_buyPanel.RequestGoBack())
            {
                _shopStateMachine.RequestStateChange(ShopStateMachine.Menu);
            }    
        }
    }
}
