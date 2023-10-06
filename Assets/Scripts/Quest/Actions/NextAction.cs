using System.Collections;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    public abstract class NextAction : ScriptableObject
    {
        public abstract IEnumerator Execute();
    }
}