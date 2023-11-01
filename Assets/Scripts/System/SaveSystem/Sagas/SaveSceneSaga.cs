using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SaveSceneSaga : SaveSagaBase<SaveSceneAction>
    {
        protected override void HandleSave(SaveSceneAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new SceneSaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SaveSceneCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SaveSceneCompletedAction(false));
        }
    }
}