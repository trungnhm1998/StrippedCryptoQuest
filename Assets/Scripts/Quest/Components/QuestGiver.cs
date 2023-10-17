using System;
using CryptoQuest.Character.DialogueProviders;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestGiver : MonoBehaviour
    {
        [SerializeField] private DialogueProviderBehaviour _dialogueProviderBehaviour;

        [Header("Quest Configs")]
        [SerializeField] private QuestSO _quest;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        private bool _canGiveQuest;

        private void Awake()
        {
            if (_quest != null) _canGiveQuest = true;
        }

        private void OnEnable()
        {
            if (_quest != null) _quest.OnQuestCompleted += OnQuestCompleted;
        }


        private void OnDisable()
        {
            if (_quest != null) _quest.OnQuestCompleted -= OnQuestCompleted;
        }


        private void OnQuestCompleted() => _canGiveQuest = false;

        public void GiveQuest()
        {
            if (_canGiveQuest)
            {
                _giveQuestEventChannel.RaiseEvent(_quest);
                return;
            }

            if (_dialogueProviderBehaviour) _dialogueProviderBehaviour.ShowDialogue();
        }
    }
}