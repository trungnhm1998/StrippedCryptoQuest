using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SaveChestSage : SaveSagaBase<SaveChestAction>
    {
        protected override void HandleSave(SaveChestAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new ChestSaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SaveChestCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SaveChestCompletedAction(false));
        }
    }
}