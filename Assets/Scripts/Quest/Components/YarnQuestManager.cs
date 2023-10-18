using System;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class YarnQuestManager : MonoBehaviour
    {
        public static Action<YarnQuestDef> OnUpdateCurrentNode;
        public static Action OnDialogCompleted;
        public static Action<string> OnQuestCompleted;

        [SerializeField] private QuestEventChannelSO _questEventChannelSo;
        [SerializeField] private VoidEventChannelSO _dialogCompletedEventChannelSo;
        [SerializeField] private StringEventChannelSO _questCompletedEventChannelSo;

        private YarnQuestDef _current;
        private QuestSO _dialogueQuestSo;

        private readonly List<DialogueQuestInfo> _currentlyProcessDialogueQuests = new();
        private readonly List<string> _currentlyProcessQuestsID = new();

        private void OnEnable()
        {
            OnUpdateCurrentNode += UpdateCurrentDef;
            OnQuestCompleted += QuestCompleted;
            _dialogCompletedEventChannelSo.EventRaised += OnDialogueCompleted;
            _questCompletedEventChannelSo.EventRaised += QuestCompleted;
        }

        private void OnDisable()
        {
            OnUpdateCurrentNode -= UpdateCurrentDef;
            OnQuestCompleted -= QuestCompleted;
            _dialogCompletedEventChannelSo.EventRaised -= OnDialogueCompleted;
            _questCompletedEventChannelSo.EventRaised -= QuestCompleted;
        }

        private void UpdateCurrentDef(YarnQuestDef yarnQuestDef)
        {
            _current = yarnQuestDef;
        }

        private void OnDialogueCompleted()
        {
            OnDialogCompleted?.Invoke();
            if (_dialogueQuestSo == null) return;
            _questEventChannelSo.RaiseEvent(_dialogueQuestSo);
        }

        public void GiveQuest(DialogueQuestInfo questInfo)
        {
            if (_currentlyProcessQuestsID.Contains(questInfo.Data.Guid)) return;
            
            _currentlyProcessQuestsID.Add(questInfo.Data.Guid);
            _currentlyProcessDialogueQuests.Add(questInfo);
        }

        public void QuestCompleted(string questName)
        {
            foreach (var possibleOutComeQuest in _currentlyProcessDialogueQuests)
            {
                if (possibleOutComeQuest.Data.QuestName != questName) continue;
                
                _questEventChannelSo.RaiseEvent(possibleOutComeQuest.Data);
                break;
            }
        }
    }
}