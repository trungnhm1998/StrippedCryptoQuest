using System;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.CutsceneSystem;
using UnityEngine;

namespace CryptoQuest.Quest.Controllers
{
    public class QuestCutsceneController : BaseQuestController
    {
        public static Action<NextAction> OnTriggerNextAction;
        public static Action<YarnQuestDef> RegisterYarnQuestDef;
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannelSo;
        [SerializeField] private QuestEventChannelSO _triggerQuestEventChannelSo;
        [SerializeField] private StringEventChannelSO _dialogueOptionQuestEventChannelSo;
        private List<QuestInfo> _branchingQuests = new();
        private List<QuestInfo> _pendingBranchingQuests = new();

        private void OnEnable()
        {
            RegisterYarnQuestDef += RegisterYarnQuest;
            OnTriggerNextAction += TriggerNextAction;
        }

        private void OnDisable()
        {
            RegisterYarnQuestDef -= RegisterYarnQuest;
            OnTriggerNextAction -= TriggerNextAction;
        }

        private void TriggerNextAction(NextAction nextAction)
        {
            StartCoroutine(nextAction.Execute());
        }

        public void GiveBranchingQuest(QuestInfo questInfo)
        {
            _branchingQuests.Add(questInfo);
        }

        private void RegisterYarnQuest(YarnQuestDef yarnQuestDef)
        {
            SubscribeYarn();
            foreach (var outcomeQuest in yarnQuestDef.PossibleOutcomeQuests)
            {
                _giveQuestEventChannelSo.RaiseEvent(outcomeQuest);
            }
        }

        private void SubscribeYarn()
        {
            _dialogueOptionQuestEventChannelSo.EventRaised += OnDialogueOptionSelected;
            CutsceneManager.CutsceneCompleted += OnQuestFinish;
        }

        private void UnsubscribeYarn()
        {
            CutsceneManager.CutsceneCompleted -= OnQuestFinish;
            _dialogueOptionQuestEventChannelSo.EventRaised -= OnDialogueOptionSelected;
        }

        private void OnDialogueOptionSelected(string questName)
        {
            foreach (var quest in _branchingQuests)
            {
                if (quest.BaseData.QuestName != questName) continue;
                _pendingBranchingQuests.Add(quest);
            }
        }

        protected override void OnQuestFinish()
        {
            ExecuteBranchingQuests();
            UnsubscribeYarn();
        }

        private void ExecuteBranchingQuests()
        {
            foreach (var quest in _pendingBranchingQuests)
            {
                _triggerQuestEventChannelSo.RaiseEvent(quest.BaseData);
            }

            _pendingBranchingQuests.Clear();
        }
    }
}