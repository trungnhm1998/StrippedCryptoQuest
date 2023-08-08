using CryptoQuest.UI.Menu.Panels.Option;

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