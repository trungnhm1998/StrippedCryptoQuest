using CryptoQuest.Shop.UI.Panels.Sell;
using UnityEngine;

namespace CryptoQuest.Shop.UI.ShopStates.Sell
{
    public class ShopSellState : ShopStateBase
    {
        private UISellPanel _uiSellPanel;

        public ShopSellState(UISellPanel panel) : base(panel)
        {
            _uiSellPanel = panel;
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

        public override void ChangeTab(float direction)
        {
            base.ChangeTab(direction);
            _uiSellPanel.ChangeTab(direction);
        }
    }
}
