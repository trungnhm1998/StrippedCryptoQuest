using CryptoQuest.UI.Menu.Panels.Option;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.OptionStates
{
    public class OptionStateBase : MenuStateBase
    {
        protected UIOptionMenu OptionPanel { get; }

        protected OptionStateBase(UIOptionMenu optionPanel)
        {
            OptionPanel = optionPanel;
        }
    }
}