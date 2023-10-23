using System;
using CryptoQuest.System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Dialogue Quest", fileName = "DialogueQuestSO")]
    public class DialogueQuestSO : QuestSO
    {
        public override QuestInfo CreateQuest()
            => new DialogueQuestInfo(this);
    }

    [Serializable]
    public class DialogueQuestInfo : QuestInfo<DialogueQuestSO>
    {
        public DialogueQuestInfo(DialogueQuestSO questSo)
            : base(questSo) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            var questManager = ServiceProvider.GetService<QuestManager>();
            var yarnQuestManager = questManager?.GetComponent<YarnQuestManager>();
            yarnQuestManager?.GiveQuest(this);
        }
    }
}