using CryptoQuest.UI.Menu.MenuStates.HomeStates;
using FSM;

namespace CryptoQuest.UI.Menu.Panels.Home
{
    public class UIHomeMenu : UIMenuPanel
    {
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new HomeMenuStateMachine(this);
        }
    }
}