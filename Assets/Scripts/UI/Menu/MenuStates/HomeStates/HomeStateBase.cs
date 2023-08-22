using CryptoQuest.UI.Menu.Panels.Home;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.HomeStates
{
    public abstract class HomeStateBase : MenuStateBase
    {
        protected UIHomeMenu HomePanel { get; }

        public HomeStateBase(UIHomeMenu homePanel)
        {
            HomePanel = homePanel;
        }
    }
}