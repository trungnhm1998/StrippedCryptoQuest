using System;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(fileName = "QuestDatabase", menuName = "Quest System/Database", order = 0)]
    public class QuestDatabase : ScriptableObject
    {
        [SerializeField] private AbstractObjective[] _quests = Array.Empty<AbstractObjective>();
        public AbstractObjective[] Quests => _quests;
    }
}