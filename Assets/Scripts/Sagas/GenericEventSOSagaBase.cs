using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public abstract class GenericEventSOSagaBase<TContext> : MonoBehaviour
    {
        [SerializeField] private GenericEventChannelSO<TContext> _action;

        protected virtual void OnEnable()
        {
            _action.EventRaised += HandleAction;
        }

        protected virtual void OnDisable()
        {
            _action.EventRaised -= HandleAction;
        }

        protected abstract void HandleAction(TContext ctx);
    }
}