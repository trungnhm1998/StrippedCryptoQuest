using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.System.SaveSystem.Actions;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public abstract class SaveSagaBase<TAction> : SagaBase<TAction> where TAction : ActionBase
    {
        protected override void HandleAction(TAction ctx)
        {
            HandleSave(ctx);
        }

        protected abstract void HandleSave(TAction ctx);
    }
}