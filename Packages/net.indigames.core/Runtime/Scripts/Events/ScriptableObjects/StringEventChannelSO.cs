using UnityEngine;
using UnityEngine.Events;

namespace IndiGames.Core.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Core/Events/String Event Channel")]
    public class StringEventChannelSO : ScriptableObject
    {
        public UnityAction<string> EventRaised;

        public void RaiseEvent(string value)
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