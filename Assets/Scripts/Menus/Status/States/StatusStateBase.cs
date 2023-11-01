using CryptoQuest.Menus.Status.UI;
using FSM;

namespace CryptoQuest.Menus.Status.States
{
    /// <summary>
    /// Every state in the status menu inherits from this class.
    /// So it can have the _panels with correct type to work with.
    /// </summary>
    public abstract class StatusStateBase : StateBase
    {
        protected UIStatusMenu StatusPanel { get; }

        protected StatusStateBase(UIStatusMenu statusPanel) : base(false)
        {
            StatusPanel = statusPanel;
        }
    }
}