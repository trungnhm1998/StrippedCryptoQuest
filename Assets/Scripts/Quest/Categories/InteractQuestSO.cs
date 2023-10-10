using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Interact Quest", fileName = "InteractQuestSO")]
    public class InteractQuestSO : QuestSO
    {
        [field: SerializeField] public YarnDialogWithQuestSo YarnDialogWithQuestSo { get; private set; }

        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new InteractQuestInfo(this, questManager);
    }

    [Serializable]
    public class InteractQuestInfo : QuestInfo<InteractQuestSO>
    {
        private readonly QuestInteractController _questInteractController;

        public InteractQuestInfo(InteractQuestSO interactQuestSO,
            QuestManager questManager) : base(
            interactQuestSO)
        {
            _questInteractController = questManager.GetComponent<QuestInteractController>();
            _questInteractController.QuestManager = questManager;
        }

        public override void TriggerQuest()
        {
            base.TriggerQuest();

            YarnQuestDef yarnDialogData = Data.YarnDialogWithQuestSo.YarnQuestDef;
            YarnQuestManager.OnUpdateCurrentNode?.Invoke(yarnDialogData);
            YarnSpinnerDialogueManager.PlayDialogueRequested.Invoke(yarnDialogData.YarnNode);
        }

        public override void FinishQuest()
        {
            base.FinishQuest();
            YarnQuestManager.OnQuestCompleted.Invoke(Data.QuestName);
        }

        public override void GiveQuest()
        {
            _questInteractController.GiveQuest(this);
            _questInteractController.TriggerQuest(this);
        }
    }
}