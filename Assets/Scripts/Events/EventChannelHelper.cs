using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    public static class EventChannelHelper
    {
        public static void CallEventSafely<T>(this ScriptableObject owner, UnityAction<T> action, T passedObject)
        {
            if (action == null)
            {
                Debug.LogWarning($"Event was raised on {owner.name} but no one was listening.");
                return;
            }

            action.Invoke(passedObject);
        }
    }
}