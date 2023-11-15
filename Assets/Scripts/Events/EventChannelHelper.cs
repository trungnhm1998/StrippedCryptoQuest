using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Events
{
    public static class UnityActionExtensions
    {
        public static void SafeInvoke<T>(this UnityAction<T> action, T value)
        {
            if (action == null)
            {
                Debug.LogWarning($"Event was raised but no one was listening.");
                return;
            }

            action.Invoke(value);
        }
    }
}