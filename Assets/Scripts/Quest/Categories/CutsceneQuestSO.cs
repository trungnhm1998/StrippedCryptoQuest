using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Cutscene Quest", fileName = "CutsceneQuestSO", order = 0)]
    public class CutsceneQuestSO : QuestSO<CutsceneQuestData>
    {
        [SerializeField] QuestCutsceneDef _cutsceneQuestSO;

        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new CutsceneQuestData(this, _cutsceneQuestSO);
    }

    [Serializable]
    public class CutsceneQuestData : QuestData<CutsceneQuestSO>
    {
        private readonly QuestCutsceneDef _cutsceneDef;

        public CutsceneQuestData(CutsceneQuestSO cutsceneQuestSO,
            QuestCutsceneDef questCutsceneDef) : base(cutsceneQuestSO)
        {
            _cutsceneDef = questCutsceneDef;
        }

        public CutsceneQuestData() { }

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
    }
}