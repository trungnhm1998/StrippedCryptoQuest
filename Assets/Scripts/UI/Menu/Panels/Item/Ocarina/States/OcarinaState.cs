using CryptoQuest.UI.Menu.MenuStates;
using UnityEngine;

namespace CryptoQuest.UI.Menu.Panels.Item.Ocarina.States
{
    public class OcarinaState : MenuStateBase
    {
        public static readonly string Ocarina = "Ocarina";

        private UIOcarinaPresenter _uiOcarinaPresenter;

        public OcarinaState(UIOcarinaPresenter uiOcarinaPresenter)
        {
            _uiOcarinaPresenter = uiOcarinaPresenter;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            Debug.Log($"OcarinaState OnEnter");
        }

        public override void HandleCancel()
        {
            base.HandleCancel();
        }
    }
}