using System;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    public abstract class Quest
    {
        public abstract void TriggerQuest();
    }
}