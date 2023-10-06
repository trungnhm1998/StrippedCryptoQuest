using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.Dialogue.Managers;

namespace CryptoQuest.Quest
{
    public class QuestInteractController : BaseQuestController
    {
        private List<InteractQuestInfo> _currentlyInteractQuests => new();
        private YarnDialogWithQuestSo _currentInteractQuest;

        public void GiveQuest(InteractQuestInfo questInfo)
        {
            _currentlyInteractQuests.Add(questInfo);
        }

        public void TriggerQuest(InteractQuestInfo questInfo)
        {
            _currentInteractQuest = questInfo.Data.YarnDialogWithQuestSo;

            YarnQuestDef yarnDialogData = questInfo.Data.YarnDialogWithQuestSo.YarnQuestDef;
            
            QuestManager.TriggerQuest(questInfo.Data);

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

            _currentlyInteractQuests.Remove(processingQuest);

            YarnQuestHandler.OnDialogCompleted -= OnQuestFinish;
        }
    }
}