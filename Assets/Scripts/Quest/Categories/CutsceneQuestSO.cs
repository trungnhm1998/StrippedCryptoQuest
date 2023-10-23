using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Cutscene Quest", fileName = "CutsceneQuestSO", order = 0)]
    public class CutsceneQuestSO : QuestSO<CutsceneQuestInfo>
    {
        [field: SerializeField] public QuestCutsceneDef CutSceneToLoad { get; private set; }

        public override QuestInfo CreateQuest() =>
            new CutsceneQuestInfo(this);
    }

    [Serializable]
    public class CutsceneQuestInfo : QuestInfo<CutsceneQuestSO>
    {
        public CutsceneQuestInfo(CutsceneQuestSO cutsceneQuestSO) : base(cutsceneQuestSO)
        {}

        public CutsceneQuestInfo() { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            Data.CutSceneToLoad.RaiseEvent();
        }
    }
}