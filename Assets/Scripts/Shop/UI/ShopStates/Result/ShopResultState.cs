using CryptoQuest.Shop.UI.Panels;
using CryptoQuest.Shop.UI.Panels.Result;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Shop.UI.ShopStates.Result
{
    public class ShopResultState : ShopStateBase
    {
        private UIResultPanel _resultPanel;

        public ShopResultState(UIResultPanel panel) : base(panel)
        {
            _resultPanel = panel;
        }

        public override void Confirm()
        {
            base.Confirm();
            _shopContext.RequestChangeState(_resultPanel.PreState);
        }

        public override void HandleBack()
        {
            base.HandleBack();
            _shopContext.RequestChangeState(_resultPanel.PreState);
        }
    }
}
