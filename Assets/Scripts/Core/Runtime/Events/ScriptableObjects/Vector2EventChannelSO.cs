using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Runtime.Events.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Events/Vector2 Event Channel")]

    public class Vector2EventChannelSO : ScriptableObject
    {
        public UnityAction<Vector2> EventRaised;

        public void RaiseEvent(Vector2 vector2)
        {
            OnRaiseEvent(vector2);
        }

        private void OnRaiseEvent(Vector2 vector2)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(vector2);
        }
    }
}
