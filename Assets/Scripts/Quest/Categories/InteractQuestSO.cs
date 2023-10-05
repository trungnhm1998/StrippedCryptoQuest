using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Interact Quest", fileName = "InteractQuestSO")]
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
        }

        public override void GiveQuest()
        {
            _questInteractController.GiveQuest(this);
            _questInteractController.TriggerQuest(this);
        }
    }
}