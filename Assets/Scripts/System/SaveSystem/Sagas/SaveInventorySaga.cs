using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SaveInventorySaga : SaveSagaBase<SaveInventoryAction>
    {
        protected override void HandleSave(SaveInventoryAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new InventorySaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SaveInventoryCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SaveInventoryCompletedAction(false));
        }
    }
}