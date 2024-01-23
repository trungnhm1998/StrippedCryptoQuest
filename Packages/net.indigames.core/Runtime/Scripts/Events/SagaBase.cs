using TinyMessenger;
using UnityEngine;

namespace IndiGames.Core.Events
{
    public abstract class SagaBase<TAction> : MonoBehaviour where TAction : ActionBase
    {
        private TinyMessageSubscriptionToken _actionToken;

        protected virtual void OnEnable() => _actionToken = ActionDispatcher.Bind<TAction>(HandleAction);

        protected virtual void OnDisable() => ActionDispatcher.Unbind(_actionToken);

        protected abstract void HandleAction(TAction ctx);
    }
    
    public abstract class CoSagaBase<TAction> : SagaBase<TAction> where TAction : ActionBase
    {
        protected override void HandleAction(TAction ctx) => StartCoroutine(HandleActionCoroutine(ctx));

        protected abstract System.Collections.IEnumerator HandleActionCoroutine(TAction ctx);
    }
}