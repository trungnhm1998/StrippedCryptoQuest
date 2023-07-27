using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.Events
{
    /// <summary>
    /// Planners friendly version 
    /// </summary>
    public class VoidEventChannelListener : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO _event;

        public UnityEvent OnEventRaised;

        private void OnEnable()
        {
            if (_event) _event.EventRaised += OnRaised;
        }

        private void OnDisable()
        {
            if (_event) _event.EventRaised -= OnRaised;
        }

        private void OnRaised()
        {
            OnEventRaised?.Invoke();
        }
    }
}