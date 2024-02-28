﻿using System;
using CryptoQuest.System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.Dialogue.Managers;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Interact Quest", fileName = "InteractQuestSO")]
    public class InteractQuestSO : QuestSO
    {
        [field: SerializeField] public YarnDialogWithQuestSo YarnDialogWithQuestSo { get; private set; }

        [field: SerializeField] public QuestEventChannelSO GiveQuestEventChannel { get; private set; }

        public override QuestInfo CreateQuest() =>
            new InteractQuestInfo(this);
    }

    [Serializable]
    public class InteractQuestInfo : QuestInfo<InteractQuestSO>
    {
        private QuestInteractController _questInteractController;
        public InteractQuestInfo(InteractQuestSO interactQuestSO) : base(interactQuestSO) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();

            YarnQuestDef yarnDialogData = Data.YarnDialogWithQuestSo.YarnQuestDef;
            YarnQuestManager.OnUpdateCurrentNode?.Invoke(yarnDialogData);
            YarnSpinnerDialogueManager.PlayDialogueRequested.Invoke(yarnDialogData.YarnNode);
            foreach (var possibleOutcomeQuest in yarnDialogData.PossibleOutcomeQuests)
            {
                Data.GiveQuestEventChannel.RaiseEvent(possibleOutcomeQuest);
            }
            
            _questInteractController?.TriggerQuest(this);
        }

        public override void FinishQuest()
        {
            base.FinishQuest();
            YarnQuestManager.OnQuestCompleted.Invoke(Data.QuestName);
        }

        public override void GiveQuest()
        {
            var questManager = ServiceProvider.GetService<QuestManager>();
            _questInteractController = questManager?.GetComponent<QuestInteractController>();
            _questInteractController?.GiveQuest(this);
        }
    }
}