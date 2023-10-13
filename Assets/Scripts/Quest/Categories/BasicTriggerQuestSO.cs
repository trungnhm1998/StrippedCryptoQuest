using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Basic Trigger Quest", fileName = "BasicTriggerQuestSO")]
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
            base.TriggerQuest();
            FinishQuest();
        }
    }
}