using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Core/Events/Int Event Channel")]
    public class IntEventChannelSO : ScriptableObject
    {
        public UnityAction<int> EventRaised;

        public void RaiseEvent(int value)
        {
            OnRaiseEvent(value);
        }

        private void OnRaiseEvent(int value)
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