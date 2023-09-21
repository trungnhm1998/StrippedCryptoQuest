using System;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.System.Dialogue.Managers;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestDialogController : MonoBehaviour
    {
        public static Action<QuestProgressionConfigs> PlayQuestDialogue;
        public PlayDialogueEvent PlayDialogueEventChannelSO;

        [SerializeField] private VoidEventChannelSO _dialogueCompletedEventChannelSO;

        private QuestProgressionConfigs _questDialogue;

        private void OnEnable()
        {
            PlayQuestDialogue += ShowQuestDialogue;
            _dialogueCompletedEventChannelSO.EventRaised += OnDialogueCompleted;
        }

        private void OnDisable()
        {
            PlayQuestDialogue -= ShowQuestDialogue;
            _dialogueCompletedEventChannelSO.EventRaised -= OnDialogueCompleted;
        }

        public void TriggerQuestDialogue(string yarnNode)
        {
            PlayDialogueEventChannelSO.RaiseEvent(yarnNode);
        }

        private void ShowQuestDialogue(QuestProgressionConfigs config)
        {
            _questDialogue = config;
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(_questDialogue.YarnNode);
        }

        private void OnDialogueCompleted()
        {
            if (_questDialogue == null) return;

            _questDialogue.Task.OnComplete();
            _questDialogue = null;
        }
    }
}