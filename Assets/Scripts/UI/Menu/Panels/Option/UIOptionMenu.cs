using CryptoQuest.UI.Menu.MenuStates.OptionStates;
using FSM;

namespace CryptoQuest.UI.Menu.Panels.Option
{
    public class UIOptionMenu : UIMenuPanel
    {
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new OptionMenuStateMachine(this);
        }
    }
}