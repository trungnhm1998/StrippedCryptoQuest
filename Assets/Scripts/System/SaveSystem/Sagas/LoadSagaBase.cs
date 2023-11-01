using CryptoQuest.Core;
using CryptoQuest.Sagas;
using CryptoQuest.System.SaveSystem.Actions;
using System.Collections;

namespace CryptoQuest.System.SaveSystem.Sagas
{
    public abstract class LoadSagaBase<TAction> : SagaBase<TAction> where TAction : ActionBase
    {
        protected override void HandleAction(TAction ctx)
        {
            StartCoroutine(CoLoadObject(ctx));
        }

        protected abstract IEnumerator CoLoadObject(TAction ctx);
    }
}