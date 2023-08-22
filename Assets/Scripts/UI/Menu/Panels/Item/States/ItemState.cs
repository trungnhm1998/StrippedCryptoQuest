using CryptoQuest.UI.Menu.MenuStates;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item.States
{
    public class ItemState : MenuStateBase
    {
        public static readonly string UseItemForSingleAlly = "Single";
        public static readonly string UseItemForAllAllies = "All";

        private UIItemPresenter _uiItemPresenter;

        public ItemState(UIItemPresenter uiItemPresenter)
        {
            _uiItemPresenter = uiItemPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _uiItemPresenter.Show();
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            _uiItemPresenter.Hide();
        }
    }
}