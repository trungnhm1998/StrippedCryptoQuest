using CryptoQuest.Quest.Authoring;

namespace CryptoQuest.Quest
{
    public interface IQuestConfigure
    {
        QuestSO QuestToTrack { get; set; }
        void Configure(bool isQuestCompleted);
    }
}