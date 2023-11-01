using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SaveQuestSaga : SaveSagaBase<SaveQuestAction>
    {
        protected override void HandleSave(SaveQuestAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new QuestSaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SaveQuestCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SaveQuestCompletedAction(false));
        }
    }
}