using FSM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CryptoQuest.Shop.UI.ShopStates.Menu;
using CryptoQuest.Shop.UI.Panels;
using CryptoQuest.Shop.UI.ScriptableObjects;

namespace CryptoQuest.Shop.UI.ShopStates
{
    public class ShopStateBase : StateBase
    {
        protected readonly UIShopPanel _panel;
        protected ShopStateMachine _shopStateMachine => (ShopStateMachine)fsm; // TODO: code smell here
        protected ShopManager _shopContext => _shopStateMachine.ShopManagerContext;

        public ShopStateBase(UIShopPanel panel) : base(false)
        {
            _panel = panel;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"{GetType().Name}/{_panel.name}/OnEnter");
            _panel.Show();
            _shopContext.ShowDialog(_panel.DiaglogMessage);
        }

        public override void OnExit()
        {
            base.OnExit();
            Debug.Log($"{GetType().Name}/{_panel.name}/OnExit");
            _panel.Hide();
        }

        public virtual void HandleBack()
        {
            Debug.Log($"{GetType().Name}/{_panel.name}/HandleBack");
        }

        public virtual void Confirm()
        {
            Debug.Log($"{GetType().Name}/{_panel.name}/Confirm");
        }

        public virtual void ChangeTab(float direction)
        {
            Debug.Log($"{GetType().Name}/{_panel.name}/ChangeTab/{direction}");
        }
    }
}
