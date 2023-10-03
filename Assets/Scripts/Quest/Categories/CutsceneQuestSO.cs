using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Cutscene Quest", fileName = "CutsceneQuestSO", order = 0)]
    public class CutsceneQuestSO : QuestSO<CutsceneQuestInfo>
    {
        [SerializeField] QuestCutsceneDef _cutsceneQuestSO;

        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new CutsceneQuestInfo(this, _cutsceneQuestSO);
    }

    [Serializable]
    public class CutsceneQuestInfo : QuestInfo<CutsceneQuestSO>
    {
        private readonly QuestCutsceneDef _cutsceneDef;

        public CutsceneQuestInfo(CutsceneQuestSO cutsceneQuestSO,
            QuestCutsceneDef questCutsceneDef) : base(cutsceneQuestSO)
        {
            _cutsceneDef = questCutsceneDef;
        }

        public CutsceneQuestInfo() { }

        public override void TriggerQuest()
        {
            if (!_cutsceneDef.PlayOnLoaded) return;
            base.TriggerQuest();

            _cutsceneDef.RaiseEvent();
            CutsceneManager.CutsceneCompleted += FinishQuest;
        }

        public override void FinishQuest()
        {
            base.FinishQuest();

            CutsceneManager.CutsceneCompleted -= FinishQuest;
        }

        public override void GiveQuest() { }
    }
}