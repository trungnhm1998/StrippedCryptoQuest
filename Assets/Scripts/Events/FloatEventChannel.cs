using System;
using UnityEngine;

namespace CryptoQuest.Events
{
    [CreateAssetMenu(menuName = "Events/Float Event Channel")]
    public class FloatEventChannel : ScriptableObject
    {
        public Action<float> OnEventRaised;

        public void RaiseEvent(float value)
        {
            OnEventRaised?.Invoke(value);
        }
    }
}
