using CryptoQuest.UI.Menu.Panels.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public abstract class ItemStateBase : MenuStateBase
    {
        protected UIConsumableMenuPanel ConsumablePanel { get; }
        
        protected ItemStateBase(UIConsumableMenuPanel consumablePanel)
        {
            ConsumablePanel = consumablePanel;
        }
    }
}