using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Sagas
{
    public abstract class VoidEventSOSagaBase<TAction> : MonoBehaviour where TAction : VoidEventChannelSO
    {
        [SerializeField] private TAction _action;

        private void OnEnable()
        {
            _action.EventRaised += HandleAction;
        }

        private void OnDisable()
        {
            _action.EventRaised -= HandleAction;
        }

        protected abstract void HandleAction();
    }
}