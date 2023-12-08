using FSM;

namespace CryptoQuest.Menus.DimensionalBox.States
{
    public class DBoxStateBase : ActionState<EState, EStateAction>
    {
        protected MenuPanel MenuPanel { get; }

        protected DBoxStateBase(MenuPanel menuPanel) : base(false)
        {
            MenuPanel = menuPanel;
        }
    }
}