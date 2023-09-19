using UnityEngine;
using UnityEngine.Assertions;

namespace CryptoQuest.Quests
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        [SerializeField] private QuestDatabase _database;
        [SerializeField] private Quest _startingQuest;


        private void Awake()
        {
            InitQuests();
        }

        private void InitQuests()
        {
            Assert.IsNotNull(_database, "Quest database is null.");
            Assert.IsNotNull(_startingQuest, "Starting quest is null.");
        }
    }
}