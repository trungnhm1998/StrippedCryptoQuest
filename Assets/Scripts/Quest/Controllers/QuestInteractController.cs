using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Components;

namespace CryptoQuest.Quest.Controllers
{
    public class QuestInteractController : BaseQuestController
    {
        private List<InteractQuestInfo> _currentlyInteractQuests = new();
        private YarnDialogWithQuestSo _currentInteractQuest;

        public void GiveQuest(InteractQuestInfo questInfo)
        {
            _currentlyInteractQuests.Add(questInfo);
        }

        public void TriggerQuest(InteractQuestInfo questInfo)
        {
            _currentInteractQuest = questInfo.Data.YarnDialogWithQuestSo;

            QuestManager.TriggerQuest(questInfo.Data);

            YarnQuestManager.OnDialogCompleted += OnQuestFinish;
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

            YarnQuestManager.OnDialogCompleted -= OnQuestFinish;
        }
    }
}