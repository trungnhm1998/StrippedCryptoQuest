using System;
using CryptoQuest.Quests;
using CryptoQuest.System.Dialogue.Managers;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestDialogController : MonoBehaviour
    {
        public static Action<QuestProgressionConfigs> PlayQuestDialogue;

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

        private void ShowQuestDialogue(QuestProgressionConfigs config)
        {
            _questDialogue = config;
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(_questDialogue.YarnNode);
        }

        private void OnDialogueCompleted()
        {
            if (_questDialogue == null) return;

            _questDialogue.Progress();
            _questDialogue = null;
        }
    }
}