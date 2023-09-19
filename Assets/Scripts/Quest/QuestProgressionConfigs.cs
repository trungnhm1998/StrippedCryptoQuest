using System;

namespace CryptoQuest.Quests
{
    [Serializable]
    public class QuestProgressionConfigs
    {
        public Quest Quest;
        public TalkToNpcTask Task;
        public string YarnNode => Task.YarnNode;

        public bool CanProgress()
        {
            if (Quest.Completed) return false;
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