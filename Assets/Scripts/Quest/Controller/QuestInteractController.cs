using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.Dialogue.Managers;
using UnityEditor;

namespace CryptoQuest.Quest
{
    public class QuestInteractController : BaseQuestController
    {
        private InteractQuestInfo[] _currentlyInteractQuests = Array.Empty<InteractQuestInfo>();
        private YarnDialogWithQuestSo _currentInteractQuest;

        public void GiveQuest(InteractQuestInfo questInfo)
        {
            ArrayUtility.Add(ref _currentlyInteractQuests, questInfo);
        }

        public void TriggerQuest(InteractQuestInfo questInfo)
        {
            TriggerQuestEventChannel.RaiseEvent(questInfo.Data);
            _currentInteractQuest = questInfo.Data.YarnDialogWithQuestSo;

            YarnQuestDef yarnDialogData = questInfo.Data.YarnDialogWithQuestSo.YarnQuestDef;

            YarnQuestHandler.OnUpdateCurrentNode?.Invoke(yarnDialogData);
            YarnSpinnerDialogueManager.PlayDialogueRequested.Invoke(yarnDialogData.YarnNode);
            YarnQuestHandler.OnDialogCompleted += OnQuestFinish;
        }

        protected override void OnQuestFinish()
        {
            foreach (var processingQuest in _currentlyInteractQuests)
            {
                if (processingQuest.Data.YarnDialogWithQuestSo != _currentInteractQuest) continue;
                HandleInteractResult(processingQuest);
                break;
            }
        }

        private void HandleInteractResult(InteractQuestInfo processingQuest)
        {
            processingQuest.FinishQuest();

            ArrayUtility.RemoveAt(ref _currentlyInteractQuests,
                Array.IndexOf(_currentlyInteractQuests, processingQuest));

            YarnQuestHandler.OnDialogCompleted -= OnQuestFinish;
        }
    }
}