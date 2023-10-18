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

        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new CutsceneQuestInfo(this, questManager);
    }

    [Serializable]
    public class CutsceneQuestInfo : QuestInfo<CutsceneQuestSO>
    {
        private readonly QuestCutsceneController _questCutsceneController;

        public CutsceneQuestInfo(CutsceneQuestSO cutsceneQuestSO,
            QuestManager questManager) : base(questManager, cutsceneQuestSO)
        {
            _questCutsceneController = questManager.GetComponent<QuestCutsceneController>();
            _questCutsceneController.QuestManager = questManager;
        }

        public CutsceneQuestInfo() { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            Data.CutSceneToLoad.RaiseEvent();
        }
    }
}