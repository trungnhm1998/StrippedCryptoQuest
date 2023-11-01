using CryptoQuest.Core;
using CryptoQuest.System.SaveSystem.Actions;
using CryptoQuest.System.SaveSystem.SaveObjects;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public class SaveLanguageSaga : SaveSagaBase<SaveLanguageAction>
    {
        protected override void HandleSave(SaveLanguageAction ctx)
        {
            var saveSystem = ServiceProvider.GetService<ISaveSystem>();
            if (saveSystem != null && ctx.RefObject != null)
            {
                var objToSave = new LanguageSaveObject(ctx.RefObject);
                if (saveSystem.SaveObject(objToSave))
                {
                    ActionDispatcher.Dispatch(new SaveLanguageCompletedAction(true));
                    return;
                }
            }
            ActionDispatcher.Dispatch(new SaveLanguageCompletedAction(false));
        }
    }
}