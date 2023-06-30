using UnityEngine;
using UnityEngine.Events;

namespace Core.Runtime.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Core/Events/Float Event Channel")]
    public class FloatEventChannelSO : ScriptableObject
    {
        public UnityAction<float> EventRaised;

        public void RaiseEvent(float value)
        {
            OnRaiseEvent(value);
        }

        private void OnRaiseEvent(float value)
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