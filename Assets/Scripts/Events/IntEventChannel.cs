using System;
using UnityEngine;

namespace CryptoQuest
{
    [CreateAssetMenu(menuName = "Events/Int Event Channel")]
    public class IntEventChannel : ScriptableObject
    {
        public Action<int> OnEventRaised;

        public void RaiseEvent(int integer)
        {
            OnEventRaised?.Invoke(integer);
        }
    }
}
