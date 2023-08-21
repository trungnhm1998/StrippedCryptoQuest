using CryptoQuest.UI.Menu.MenuStates;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item.States
{
    public class HerbState : MenuStateBase
    {
        public static readonly string Herb = "Herb";

        private UIHerbPresenter _uiHerbPresenter;

        public HerbState(UIHerbPresenter uiHerbPresenter)
        {
            _uiHerbPresenter = uiHerbPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            _uiHerbPresenter.Show();
            Debug.Log($"HerbState OnEnter");
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
            _uiHerbPresenter.Hide();
        }
    }
}