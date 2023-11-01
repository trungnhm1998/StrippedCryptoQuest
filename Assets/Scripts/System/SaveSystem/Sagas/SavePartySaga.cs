using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SavePartySaga : SaveSagaBase<SavePartyAction>
    {
        protected override void HandleSave(SavePartyAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new PartySaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SavePartyCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SavePartyCompletedAction(false));
        }
    }
}