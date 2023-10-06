using System;
using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Quest.Components
{
    public class YarnQuestManager : MonoBehaviour
    {
        public static Action<YarnQuestDef> OnUpdateCurrentNode;
        public static Action OnDialogCompleted;
        public static Action<string> OnQuestCompleted;
        [SerializeField] private QuestEventChannelSO _questEventChannelSo;
        [SerializeField] private VoidEventChannelSO _dialogCompletedEventChannelSo;
        private YarnQuestDef _current;
        private List<DialogueQuestInfo> _currentlyProcessDialogueQuests = new();
        private QuestSO _dialogueQuestSo;

        private void OnEnable()
        {
            OnUpdateCurrentNode += UpdateCurrentDef;
            OnQuestCompleted += QuestCompleted;
            _dialogCompletedEventChannelSo.EventRaised += OnDialogueCompleted;
        }

        private void OnDisable()
        {
            OnUpdateCurrentNode -= UpdateCurrentDef;
            OnQuestCompleted -= QuestCompleted;
            _dialogCompletedEventChannelSo.EventRaised -= OnDialogueCompleted;
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
            if (_currentlyProcessDialogueQuests.Contains(questInfo)) return;
            _currentlyProcessDialogueQuests.Add(questInfo);
        }

        public void QuestCompleted(string questName)
        {
            // if (_current == null) return;
            foreach (var possibleOutComeQuest in _currentlyProcessDialogueQuests)
            {
                if (possibleOutComeQuest.Data.QuestName == questName)
                    _dialogueQuestSo = possibleOutComeQuest.Data;
            }

            // _current = null;
        }
    }
}