using CryptoQuest.UI.Menu.Panels.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public class SingleItemState : MenuStateBase
    {
        public static readonly string Item = "ItemForSingleAllyState";

        private UIItemPresenter _uiGroupPresenter;

        public SingleItemState(UIItemPresenter uiGroupPresenter)
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