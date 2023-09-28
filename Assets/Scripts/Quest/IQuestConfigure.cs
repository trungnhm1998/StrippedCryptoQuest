using CryptoQuest.Quest.Authoring;

namespace CryptoQuest.Quest
{
    public interface IQuestConfigure
    {
        QuestSO Quest { get; set; }
        bool IsQuestCompleted { get; set; }
        void Configure();
    }
}