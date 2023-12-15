using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;

namespace CryptoQuest.Quest
{
    public interface IQuestConfigure
    {
        EConditionType QuestCondition { get; set; }
        List<QuestSO> QuestsToTrack { get; set; }
        void Configure(bool isQuestCompleted, int questCompletedCount);
    }
}