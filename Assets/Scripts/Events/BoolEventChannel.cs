using System;
using UnityEngine;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Events/Bool Event Channel")]
    public class BoolEventChannel : ScriptableObject
    {
        public Action<bool> OnEventRaised;

        public void RaiseEvent(bool isBool)
        {
            OnEventRaised?.Invoke(isBool);
        }
    }
}
