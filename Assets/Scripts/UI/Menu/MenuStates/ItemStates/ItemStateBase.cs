using CryptoQuest.UI.Menu.Panels.Item;
using UnityEngine;

namespace CryptoQuest.UI.Menu.MenuStates.ItemStates
{
    public abstract class ItemStateBase : MenuStateBase
    {
        protected UIConsumableMenuPanel _consumablePanel;
        
        protected ItemStateBase(UIConsumableMenuPanel consumablePanel)
        {
            _consumablePanel = consumablePanel;
        }
    }
}