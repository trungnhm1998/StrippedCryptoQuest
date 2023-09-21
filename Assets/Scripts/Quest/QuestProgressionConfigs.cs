using System;
using CryptoQuest.Quest.Authoring;

namespace CryptoQuest.Quest
{
    [Serializable]
    public class QuestProgressionConfigs
    {
        public Authoring.Quest Quest;
        public TalkToNpcTask Task;
        public string YarnNode => Task.YarnNode;

        public bool CanProgress()
        {
            if (Quest.IsCompleted) return false;
            if (!Quest.CanCompleteTask(Task)) return false;
            if (Quest.HasTaskCompleted(Task)) return false;

            return true;
        }

        public void Progress()
        {
            if (!CanProgress()) return;

            Quest.CompleteTask(Task);
        }
    }
}