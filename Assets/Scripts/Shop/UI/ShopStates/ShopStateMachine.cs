using FSM;
using UnityEngine;

namespace CryptoQuest.Shop.UI.ShopStates
{
    public class ShopStateMachine : StateMachine
    {

        public static readonly string Menu = "Menu";
        public static readonly string Buy = "Buy";
        public static readonly string Sell = "Sell";
        public static readonly string Result = "Result";
        public ShopManager ShopManagerContext { get; }
        private new ShopStateBase ActiveState => (ShopStateBase)base.ActiveState;

        public ShopStateMachine(ShopManager shopManagerContext) : base(false)
        {
            ShopManagerContext = shopManagerContext;
        }

        #region Specific logics to delegate

        public void HandleBack()
        {
            ActiveState.HandleBack();
        }

        public void ChangeTab(float direction)
        {
            ActiveState.ChangeTab(direction);
        }
        public void Confirm()
        {
            ActiveState.Confirm();
        }
        #endregion

        public override void OnEnter()
        {
            Debug.Log($"{GetType().Name}::OnEnter");
            base.OnEnter();
        }

        public override void OnExit()
        {
            Debug.Log($"{GetType().Name}::OnExit");
            base.OnExit();
        }
    }
}
