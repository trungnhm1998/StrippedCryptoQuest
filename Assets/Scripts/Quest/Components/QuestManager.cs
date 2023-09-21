using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestDatabase _database;

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