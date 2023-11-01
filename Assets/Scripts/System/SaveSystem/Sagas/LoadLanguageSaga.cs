using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class LoadLanguageSaga : LoadSagaBase<LoadLanguageAction>
    {
        protected override IEnumerator CoLoadObject(LoadLanguageAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            var objToLoad = new LanguageSaveObject(ctx.RefObject);
            if (saveSystem != null && objToLoad != null)
            {
                yield return saveSystem.CoLoadObject(objToLoad, LoadObjectCallback);
                yield break;
            }
            LoadObjectCallback(false);
        }

        protected void LoadObjectCallback(bool result)
        {
            ActionDispatcher.Dispatch(new LoadLanguageCompletedAction(result));
        }
    }
}