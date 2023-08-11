using System.Collections.Generic;
using CryptoQuest.Gameplay.Inventory.ScriptableObjects.Item;

namespace CryptoQuest.UI.Menu.Panels.Item
{
    public interface IConsumablesProvider
    {
        public List<UsableInfo> Items { get; }
    }
}