using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Events/Localized String Event Channel")]
    public class LocalizedStringEventChannelSO : ScriptableObject
    {
        public UnityAction<LocalizedString> EventRaised;
#if UNITY_EDITOR
        [SerializeField] private LocalizedString _debugValue;
        public LocalizedString DebugValue => _debugValue;
#endif

        public void RaiseEvent(LocalizedString value)
        {
            OnRaiseEvent(value);
            _debugValue = value;
        }

        private void OnRaiseEvent(LocalizedString value)
        {
            if (EventRaised == null)
            {
                Debug.LogWarning($"Event was raised on {name} but no one was listening.");
                return;
            }

            EventRaised.Invoke(value);
            _debugValue = value;
        }
    }
}