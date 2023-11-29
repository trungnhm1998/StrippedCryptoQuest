using CryptoQuest.Menus.Beast.UI;
using FSM;

namespace CryptoQuest.Menus.Beast.States
{
    public class BeastStateBase : StateBase
    {
        protected UIBeastMenu _beastPanel { get; }

        protected BeastStateBase(UIBeastMenu beastPanel) : base(false)
        {
            _beastPanel = beastPanel;
        }
    }
}