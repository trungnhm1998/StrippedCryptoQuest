using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestDatabase _database;
        [SerializeField] private List<string> _completedQuests; // GUIDs
        public List<string> CompletedQuests => _completedQuests;

        private void OnEnable()
        {
            foreach (var quest in _database.Quests)
            {
                quest.SubscribeObjective();
            }
        }

        private void OnDisable()
        {
            foreach (var quest in _database.Quests)
            {
                quest.UnsubscribeObjective();
            }
        }
    }
}