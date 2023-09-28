using System;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    public abstract class QuestInfo
    {
        public abstract QuestSO GetBaseData();
        public abstract void TriggerQuest();
        public abstract void FinishQuest();
        public abstract bool IsValid();
    }
}