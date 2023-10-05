using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Cutscene Quest", fileName = "CutsceneQuestSO", order = 0)]
    public class CutsceneQuestSO : QuestSO<CutsceneQuestInfo>
    {
        [field: SerializeField] public QuestCutsceneDef CutSceneToLoad { get; private set; }

        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new CutsceneQuestInfo(this, questManager);
    }

    [Serializable]
    public class CutsceneQuestInfo : QuestInfo<CutsceneQuestSO>
    {
        private readonly QuestCutsceneController _questCutsceneController;

        public CutsceneQuestInfo(CutsceneQuestSO cutsceneQuestSO,
            QuestManager questManager) : base(cutsceneQuestSO)
        {
            _questCutsceneController = questManager.GetComponent<QuestCutsceneController>();
        }

        public CutsceneQuestInfo() { }

        public override void GiveQuest()
        {
            _questCutsceneController.GiveQuest(this);
            _questCutsceneController.TriggerCutscene(this);
        }
    }
}