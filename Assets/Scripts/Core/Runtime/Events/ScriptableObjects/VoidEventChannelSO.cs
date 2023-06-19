using UnityEngine;
using UnityEngine.Events;

namespace Core.Runtime.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Void Event Channel")]
    public class VoidEventChannelSO : ScriptableObject
    {
        public UnityAction EventRaised;

        public void OnRaiseEvent()
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }
            EventRaised.Invoke();
        }
    }
}