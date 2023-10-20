using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;

namespace CryptoQuest.Quest
{
    public interface IQuestConfigure
    {
        List<QuestSO> QuestsToTrack { get; set; }
        void Configure(bool isQuestCompleted);
    }
}