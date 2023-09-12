using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.Events.ScriptableObjects
{
    public abstract class GenericEventChannelSO<T> : ScriptableObject
    {
        public UnityAction<T> EventRaised;

        public virtual void RaiseEvent(T obj)
        {
            OnRaiseEvent(obj);
        }

        protected virtual void OnRaiseEvent(T obj)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(obj);
        }
    }
}