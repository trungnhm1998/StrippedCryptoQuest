using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Basic Trigger Quest", fileName = "BasicTriggerQuestSO")]
    public class BasicTriggerQuestSO : QuestSO
    {
        public override QuestInfo CreateQuest() =>
            new BasicTriggerQuestInfo(this);
    }

    [Serializable]
    public class BasicTriggerQuestInfo : QuestInfo<BasicTriggerQuestSO>
    {
        public BasicTriggerQuestInfo(BasicTriggerQuestSO basicTriggerQuestSO)
            : base(basicTriggerQuestSO) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }
    }
}