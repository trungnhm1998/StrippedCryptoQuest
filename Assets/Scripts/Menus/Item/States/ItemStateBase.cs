using CryptoQuest.Menus.Item.UI;
using FSM;

namespace CryptoQuest.Menus.Item.States
{
    public abstract class ItemStateBase : StateBase
    {
        protected UIConsumableMenuPanel ConsumablePanel { get; }

        protected ItemStateBase(UIConsumableMenuPanel consumablePanel) : base(false)
        {
            ConsumablePanel = consumablePanel;
        }
    }
}