using FSM;

namespace CryptoQuest.UI.Menu.Panels.Beast
{
    public class UIBeastMenu : UIMenuPanel
    {
        public override StateBase<string> GetPanelState(MenuManager menuManager)
        {
            return new StateMachine();
        }
    }
}