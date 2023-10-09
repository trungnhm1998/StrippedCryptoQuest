using System.Collections;
using IndiGames.Core.SaveSystem.ScriptableObjects;

namespace CryptoQuest.Quest.Actions
{
    public abstract class NextAction : SerializableScriptableObject
    {
        public abstract IEnumerator Execute();
    }
}