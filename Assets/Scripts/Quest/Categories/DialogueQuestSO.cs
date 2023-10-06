using System;
using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;

namespace CryptoQuest.Quest.Categories
{
    public class DialogueQuestSO : QuestSO
    {
        public NextAction NextAction;

        public override QuestInfo CreateQuest(QuestManager questManager)
            => new DialogueQuestInfo(questManager, this);
    }

    [Serializable]
    public class DialogueQuestInfo : QuestInfo<DialogueQuestSO>
    {
        private YarnQuestManager _yarnQuestManager;

        public DialogueQuestInfo(QuestManager questManager, DialogueQuestSO questSo)
            : base(questManager, questSo) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            _yarnQuestManager = _questManager.GetComponent<YarnQuestManager>();
            _yarnQuestManager.GiveQuest(this);
        }
    }
}