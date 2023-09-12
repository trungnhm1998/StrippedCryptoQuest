using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Core/Events/Bool Event Channel")]
    public class BoolEventChannelSO : ScriptableObject
    {
        public UnityAction<bool> EventRaised;

        public void RaiseEvent(bool value)
        {
            OnRaiseEvent(value);
        }

        private void OnRaiseEvent(bool value)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised?.Invoke(value);
        }
    }
}