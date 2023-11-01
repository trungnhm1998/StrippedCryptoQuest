using CryptoQuest.Core;
using CryptoQuest.Gameplay.Inventory;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SaveInventoryAction : SaveActionBase<InventoryController>
    {
        public SaveInventoryAction(InventoryController obj) : base(obj)
        {
        }
    }

    public class LoadInventoryAction : SaveActionBase<InventoryController>
    {
        public LoadInventoryAction(InventoryController obj) : base(obj)
        {
        }
    }

    public class SaveInventoryCompletedAction : SaveCompletedActionBase
    {
        public SaveInventoryCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadInventoryCompletedAction : SaveCompletedActionBase
    {
        public LoadInventoryCompletedAction(bool result) : base(result)
        {
        }
    }
}