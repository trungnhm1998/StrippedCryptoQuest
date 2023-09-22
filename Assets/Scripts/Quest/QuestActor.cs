using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestActor : MonoBehaviour 
    {
        [SerializeField] private Authoring.Quest _quest;

        private Task _currentTask;
        public void Interact()
        {
            if (_quest == null) return;

            _currentTask = _quest.GetCurrentTask();

            if (_currentTask == null)
            {
                Debug.LogWarning($"There is no task to complete in quest: <color=red>{_quest.name}</color>.");
                return;
            }

            if (!_quest.HasQuestCompleted(_currentTask)) return;

            _quest.CompleteTask(_currentTask);
        }
    }
}