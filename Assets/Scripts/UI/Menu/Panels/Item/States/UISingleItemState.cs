using CryptoQuest.UI.Menu.MenuStates;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item.States
{
    public class UISingleItemState : MenuStateBase
    {
        public static readonly string Item = "Item";

        private UIItemPresenter _uiGroupPresenter;

        public UISingleItemState(UIItemPresenter uiGroupPresenter)
        {
            _uiGroupPresenter = uiGroupPresenter;
            
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _uiGroupPresenter.Show();
            Debug.Log($"UISingleItemState OnEnter");
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            _uiGroupPresenter.Hide();
        }
    }
}