using CryptoQuest.UI.Menu.Panels.Beast;

namespace CryptoQuest.UI.Menu.MenuStates.BeastStates
{
    public class BeastStateBase : MenuStateBase
    {
        protected UIBeastMenu BeastPanel { get; }

        protected BeastStateBase(UIBeastMenu beastPanel)
        {
            BeastPanel = beastPanel;
        }
    }
}