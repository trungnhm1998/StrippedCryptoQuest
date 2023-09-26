using System;
using CryptoQuest.Quest.Components;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
    public abstract class QuestSO : SerializableScriptableObject
    {
        public Action OnQuestCompleted;
        public abstract Quest CreateQuest(QuestManager questManager);
    }
}