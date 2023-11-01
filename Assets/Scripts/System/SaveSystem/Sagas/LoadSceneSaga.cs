using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class LoadQuestSaga : LoadSagaBase<LoadQuestAction>
    {
        protected override IEnumerator CoLoadObject(LoadQuestAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            var objToLoad = new QuestSaveObject(ctx.RefObject);
            if(saveSystem != null && objToLoad != null)
            {
                yield return saveSystem.CoLoadObject(objToLoad, LoadObjectCallback);
                yield break;
            }
            LoadObjectCallback(false);
        }

        protected void LoadObjectCallback(bool result)
        {
            ActionDispatcher.Dispatch(new LoadQuestCompletedAction(result));
        }
    }
}