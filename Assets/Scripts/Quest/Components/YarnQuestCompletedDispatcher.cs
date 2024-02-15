using System;
using System.Collections.Generic;
using CryptoQuest.Events;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Categories;
using CryptoQuest.Quest.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.Quest.Components
{
    public class YarnQuestCompletedDispatcher : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSO _questCompletedEventChannelSo;
        [SerializeField] private QuestSO _questToCheck;
        [SerializeField] private UnityEvent _onQuestCompleted;
        
        private void OnEnable()
        {
            _questCompletedEventChannelSo.EventRaised += QuestCompleted;
        }

        private void OnDisable()
        {
            _questCompletedEventChannelSo.EventRaised -= QuestCompleted;
        }

        private void QuestCompleted(string questName)
        {
            if (_questToCheck.QuestName != questName) return;
            _onQuestCompleted.Invoke();
        }
        
    }
}