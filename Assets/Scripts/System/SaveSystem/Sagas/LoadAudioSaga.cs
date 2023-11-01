using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class LoadAudioSaga : LoadSagaBase<LoadAudioAction>
    {
        protected override IEnumerator CoLoadObject(LoadAudioAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            var objToLoad = new AudioSaveObject(ctx.RefObject);
            if (saveSystem != null && objToLoad != null)
            {
                yield return saveSystem.CoLoadObject(objToLoad, LoadObjectCallback);
                yield break;
            }
            LoadObjectCallback(false);
        }

        protected void LoadObjectCallback(bool result)
        {
            ActionDispatcher.Dispatch(new LoadAudioCompletedAction(result));
        }
    }
}