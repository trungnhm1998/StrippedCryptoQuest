using CryptoQuest.Gameplay.Loot;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SaveChestAction : SaveActionBase<ChestManager>
    {
        public SaveChestAction(ChestManager obj): base(obj)
        {
        }
    }

    public class LoadChestAction : SaveActionBase<ChestManager>
    {
        public LoadChestAction(ChestManager obj): base (obj)
        {
        }
    }

    public class SaveChestCompletedAction : SaveCompletedActionBase
    {
        public SaveChestCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadChestCompletedAction : SaveCompletedActionBase
    {
        public LoadChestCompletedAction(bool result) : base(result)
        {
        }
    }
}