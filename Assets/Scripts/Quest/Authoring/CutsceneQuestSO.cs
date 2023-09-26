using System;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System", fileName = "CutsceneQuestSO", order = 0)]
    public class CutsceneQuestSO : QuestSO
    {
        [SerializeField] QuestCutsceneDef _cutsceneQuestSO;
        public override Quest CreateQuest(QuestManager questManager) => new CutsceneQuest(this, _cutsceneQuestSO);
    }

    [Serializable]
    public class CutsceneQuest : Quest
    {
        private readonly CutsceneQuestSO _questDef;
        private readonly QuestCutsceneDef _cutsceneDef;

        public CutsceneQuest(CutsceneQuestSO cutsceneQuestSO, QuestCutsceneDef questCutsceneDef)
        {
            _questDef = cutsceneQuestSO;
            _cutsceneDef = questCutsceneDef;
        }

        public override void TriggerQuest()
        {
            if (!_cutsceneDef.PlayOnLoaded) return;
            Debug.Log($"<color=green>Playing cutscene: {_questDef.name}</color>");

            _cutsceneDef.RaiseEvent();
            CutsceneManager.CutsceneCompleted += FinishTask;
        }

        private void FinishTask()
        {
            Debug.Log($"<color=green>Finished cutscene: {_questDef.name}</color>");
            _questDef.OnQuestCompleted?.Invoke();

            CutsceneManager.CutsceneCompleted -= FinishTask;
        }
    }
}