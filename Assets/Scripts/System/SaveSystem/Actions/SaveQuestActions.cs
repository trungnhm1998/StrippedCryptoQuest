using CryptoQuest.Quest.Components;

namespace CryptoQuest.System.SaveSystem.Actions
{
    public class SaveQuestAction : SaveActionBase<QuestManager>
    {
        public SaveQuestAction(QuestManager obj) : base(obj)
        {
        }
    }

    public class LoadQuestAction : SaveActionBase<QuestManager>
    {
        public LoadQuestAction(QuestManager obj) : base(obj)
        {
        }
    }

    public class SaveQuestCompletedAction : SaveCompletedActionBase
    {
        public SaveQuestCompletedAction(bool result) : base(result)
        {
        }
    }

    public class LoadQuestCompletedAction : SaveCompletedActionBase
    {
        public LoadQuestCompletedAction(bool result) : base(result)
        {
        }
    }
}