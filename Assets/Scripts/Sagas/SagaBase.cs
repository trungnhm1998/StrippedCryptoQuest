using IndiGames.Core.Events;
using TinyMessenger;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public abstract class SagaBase<TAction> : MonoBehaviour where TAction : ActionBase
    {
        private TinyMessageSubscriptionToken _actionToken;

        protected virtual void OnEnable() => _actionToken = ActionDispatcher.Bind<TAction>(HandleAction);

        protected virtual void OnDisable() => ActionDispatcher.Unbind(_actionToken);

        protected abstract void HandleAction(TAction ctx);
    }
}