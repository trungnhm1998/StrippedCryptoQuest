using System;
using CryptoQuest.System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Cutscene Branching Quest",
        fileName = "CutsceneBranchingQuestSO")]
    public class CutsceneBranchingQuestSO : QuestSO
    {
        public override QuestInfo CreateQuest()
            => new CutsceneBranchingQuestInfo(this);
    }

    [Serializable]
    public class CutsceneBranchingQuestInfo : QuestInfo<CutsceneBranchingQuestSO>
    {        public CutsceneBranchingQuestInfo(CutsceneBranchingQuestSO questSo)
            : base(questSo)
        {
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            var questManager = ServiceProvider.GetService<IQuestManager>();
            var questCutsceneController = questManager?.GetComponent<QuestCutsceneController>();
            questCutsceneController?.GiveBranchingQuest(this);
        }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }
    }
}