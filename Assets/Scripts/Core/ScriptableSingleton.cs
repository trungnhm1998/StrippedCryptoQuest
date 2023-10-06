using UnityEngine;

namespace CryptoQuest.Core
{
    public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T instance => _instance;

        private void OnEnable()
        {
            _instance = this as T;
            OnLoaded();
        }

        protected virtual void OnLoaded() { }
    }
}