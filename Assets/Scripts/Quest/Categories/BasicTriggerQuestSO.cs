using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;

namespace CryptoQuest.Quest.Categories
{
    public class BasicTriggerQuestSO : QuestSO
    {
        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new BasicTriggerQuestInfo(questManager, this);
    }

    [Serializable]
    public class BasicTriggerQuestInfo : QuestInfo<BasicTriggerQuestSO>
    {
        public BasicTriggerQuestInfo(QuestManager questManager, BasicTriggerQuestSO basicTriggerQuestSO)
            : base(questManager, basicTriggerQuestSO) { }

        public override void TriggerQuest()
        {
            FinishQuest();
            if (Data.NextAction != null)
                _questManager.StartCoroutine(Data.NextAction.Execute());
        }
    }
}