using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [Serializable]
    public class QuestSaveSO : ScriptableObject
    {
        public event Action Changed;

        public List<string> InProgressQuest = new();

        public List<string> CompletedQuests = new();

        public void AddInProgressQuest(string guid)
        {
            if (!InProgressQuest.Contains(guid))
            {
                InProgressQuest.Add(guid);
                Changed?.Invoke();
            }
        }

        public void RemoveInProgressQuest(string guid)
        {
            if (InProgressQuest.Contains(guid))
            {
                InProgressQuest.Remove(guid);
                Changed?.Invoke();
            }
        }

        public void AddCompleteQuest(string guid)
        {
            if (InProgressQuest.Contains(guid))
            {
                InProgressQuest.Remove(guid);
            }

            if (!CompletedQuests.Contains(guid))
            {
                CompletedQuests.Add(guid);
            }

            Changed?.Invoke();
        }
    }
}