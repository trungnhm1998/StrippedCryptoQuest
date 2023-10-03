using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Interact Quest", fileName = "InteractQuestSO")]
    public class InteractQuestSO : QuestSO
    {
        [field: SerializeField] public YarnDialogWithQuestSo YarnDialogWithQuestSo { get; private set; }

        public override QuestInfo CreateQuest(QuestManager questManager) =>
            new InteractQuestInfo(this, YarnDialogWithQuestSo);
    }

    [Serializable]
    public class InteractQuestInfo : QuestInfo<InteractQuestSO>
    {
        private readonly YarnDialogWithQuestSo _yarnDialogWithQuestSo;

        public InteractQuestInfo(InteractQuestSO interactQuestSO, YarnDialogWithQuestSo yarnDialogWithQuestSo) : base(
            interactQuestSO)
        {
            _yarnDialogWithQuestSo = yarnDialogWithQuestSo;
        }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            YarnQuestHandler.OnUpdateCurrentNode?.Invoke(_yarnDialogWithQuestSo.YarnQuestDef);
            YarnSpinnerDialogueManager.PlayDialogueRequested.Invoke(_yarnDialogWithQuestSo.YarnQuestDef.YarnNode);
            YarnQuestHandler.OnDialogCompleted += FinishQuest;
        }

        public override void FinishQuest()
        {
            base.FinishQuest();

            YarnQuestHandler.OnDialogCompleted -= FinishQuest;
        }

        public override void GiveQuest() { }
    }
}