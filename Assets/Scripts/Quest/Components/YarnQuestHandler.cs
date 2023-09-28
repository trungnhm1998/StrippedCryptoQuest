using System;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class YarnQuestHandler : MonoBehaviour
    {
        public static Action<YarnQuestDef> OnUpdateCurrentNode;
        public static Action OnDialogCompleted;
        public static Action<string> OnQuestCompleted;
        [SerializeField] private QuestTriggerEventChannelSO _questTriggerEventChannelSo;
        [SerializeField] private VoidEventChannelSO _dialogCompletedEventChannelSo;
        private YarnQuestDef _current;

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
        }

        public void QuestCompleted(string questName)
        {
            if (_current == null) return;
            foreach (var possibleOutComeQuest in _current.PossibleOutcomeQuests)
            {
                if (possibleOutComeQuest.QuestName == questName)
                    _questTriggerEventChannelSo.RaiseEvent(possibleOutComeQuest);
            }

            _current = null;
        }
    }
}