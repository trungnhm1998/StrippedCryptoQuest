using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Cutscene Branching Quest",
        fileName = "CutsceneBranchingQuestSO")]
    public class CutsceneBranchingQuestSO : QuestSO
    {
        public override QuestInfo CreateQuest(QuestManager questManager)
            => new CutsceneBranchingQuestInfo(questManager, this);
    }

    [Serializable]
    public class CutsceneBranchingQuestInfo : QuestInfo<CutsceneBranchingQuestSO>
    {
        private readonly QuestCutsceneController _questCutsceneController;

        public CutsceneBranchingQuestInfo(QuestManager questManager, CutsceneBranchingQuestSO questSo)
            : base(questManager, questSo)
        {
            _questCutsceneController = questManager.GetComponent<QuestCutsceneController>();
            _questCutsceneController.QuestManager = questManager;
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            _questCutsceneController.GiveBranchingQuest(this);
        }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }
    }
}