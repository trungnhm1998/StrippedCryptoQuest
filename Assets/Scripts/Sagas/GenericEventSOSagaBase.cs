using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public abstract class GenericEventSOSagaBase<TContext> : MonoBehaviour
    {
        [SerializeField] private GenericEventChannelSO<TContext> _action;
        
        private void OnEnable()
        {
            _action.EventRaised += HandleAction;
        }
        
        private void OnDisable()
        {
            _action.EventRaised -= HandleAction;
        }

        protected abstract void HandleAction(TContext ctx);
    }
}