using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.Events
{
    public class GenericEventChannelListener<TEventChannelSO, TType> : MonoBehaviour where TEventChannelSO : GenericEventChannelSO<TType>
    {
        [SerializeField] private TEventChannelSO _event;

        public UnityEvent<TType> OnEventRaised;

        private void OnEnable()
        {
            if (_event) _event.EventRaised += OnRaised;
        }

        private void OnDisable()
        {
            if (_event) _event.EventRaised -= OnRaised;
        }

        private void OnRaised(TType type)
        {
            OnEventRaised?.Invoke(type);
        }
    }
}