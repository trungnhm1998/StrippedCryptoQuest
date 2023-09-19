using System;
using UnityEngine;

namespace CryptoQuest.Quests
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "Quest System/Database", order = 0)]
    public class QuestDatabase : ScriptableObject
    {
        [SerializeField] private Quest[] _quests = Array.Empty<Quest>();
        public Quest[] Quests => _quests;
    }
}