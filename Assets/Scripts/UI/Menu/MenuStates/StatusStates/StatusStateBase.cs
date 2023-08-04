using CryptoQuest.UI.Menu.Panels.Status;

namespace CryptoQuest.UI.Menu.MenuStates.StatusStates
{
    /// <summary>
    /// Every state in the status menu inherits from this class.
    /// So it can have the _panels with correct type to work with.
    /// </summary>
    public abstract class StatusStateBase : MenuStateBase
    {
        protected UIStatusMenu StatusPanel { get; }

        protected StatusStateBase(UIStatusMenu statusPanel)
        {
            StatusPanel = statusPanel;
        }
    }
}