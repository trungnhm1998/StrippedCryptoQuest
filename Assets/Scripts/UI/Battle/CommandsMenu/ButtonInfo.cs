using System;
using UnityEngine.Events;

namespace CryptoQuest.UI.Battle
{
    [Serializable]
    public class ButtonInfo
    {
        public string Name;
        public string Value;
        public UnityEvent Callback;
    }
}