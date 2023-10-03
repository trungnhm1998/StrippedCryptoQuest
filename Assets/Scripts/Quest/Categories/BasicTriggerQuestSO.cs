using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;

namespace CryptoQuest.Quest.Categories
{
    public class BasicTriggerQuestSO : QuestSO
    {
        public override QuestInfo CreateQuest(QuestManager questManager) => new BasicTriggerQuestData(this);
    }

    [Serializable]
    public class BasicTriggerQuestData : QuestData<BasicTriggerQuestSO>
    {
        public BasicTriggerQuestData(BasicTriggerQuestSO basicTriggerQuestSO) : base(basicTriggerQuestSO) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }

        public override void GiveQuest() { }
    }
}