using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SaveAudioSaga : SaveSagaBase<SaveAudioAction>
    {
        protected override void HandleSave(SaveAudioAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new AudioSaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SaveAudioCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SaveAudioCompletedAction(false));
        }
    }
}