using System;
using CryptoQuest.Quest.Categories;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEditor;

namespace CryptoQuest.Quest
{
    public class QuestCutsceneController : BaseQuestController
    {
        private CutsceneQuestInfo[] _currentlyCutsceneQuests = Array.Empty<CutsceneQuestInfo>();
        private QuestCutsceneDef _currentCutscene;

        public void GiveQuest(CutsceneQuestInfo questInfo)
        {
            ArrayUtility.Add(ref _currentlyCutsceneQuests, questInfo);
        }

        public void TriggerCutscene(CutsceneQuestInfo questInfo)
        {
            TriggerQuestEventChannel.RaiseEvent(questInfo.Data);

            _currentCutscene = questInfo.Data.CutSceneToLoad;
            _currentCutscene.RaiseEvent();

            CutsceneManager.CutsceneCompleted += OnQuestFinish;
        }

        protected override void OnQuestFinish()
        {
            foreach (var processingQuest in _currentlyCutsceneQuests)
            {
                if (processingQuest.Data.CutSceneToLoad != _currentCutscene) continue;
                HandleCutsceneResult(processingQuest);
                break;
            }
        }

        private void HandleCutsceneResult(CutsceneQuestInfo processingQuest)
        {
            processingQuest.FinishQuest();
            ArrayUtility.RemoveAt(ref _currentlyCutsceneQuests,
                Array.IndexOf(_currentlyCutsceneQuests, processingQuest));
            CutsceneManager.CutsceneCompleted -= OnQuestFinish;
        }
    }
}