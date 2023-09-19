using System;
using System.Collections.Generic;
using UnityEngine;

namespace CryptoQuest.Quests
{
    [Serializable]
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
    public class Quest : ScriptableObject
    {
        public event IQuestDefinition.StatusChangedEvent StatusChanged;

        [SerializeField] private string _id;

        /// <summary>
        /// Only check objectives if this is false..
        /// </summary>
        public bool Completed;

        [Header("Details")]
        [SerializeField] private int _currentObjectiveIndex;

        [SerializeField, NonReorderable] private List<TaskContainer> _tasks = new();

        private void OnEnable()
        {
            StatusChanged += OnStatusChanged;
        }

        private void OnDisable()
        {
            StatusChanged -= OnStatusChanged;
        }

        private void OnStatusChanged(bool completed)
        {
            if (StatusChanged == null)
            {
                Debug.LogWarning("Quest status changed, but no listeners were found.");
                return;
            }

            StatusChanged(completed);
        }

        public bool HasTaskCompleted(Task task) => false;

        public bool CanCompleteTask(Task task)
        {
            for (var index = 0; index < _tasks.Count; index++)
            {
                var configTask = _tasks[index];
                if (configTask.Task.CompareTo(task) != 0) continue;

                if (index <= 0) continue;

                var previousTask = _tasks[index - 1];
                if (!previousTask.Completed)
                {
                    return false;
                }
            }

            return true;
        }

        public void CompleteTask(Task task)
        {
            for (var index = 0; index < _tasks.Count; index++)
            {
                var configTask = _tasks[index];
                if (configTask.Task.CompareTo(task) != 0) continue;
                configTask.Completed = true;

                if (index == _tasks.Count - 1)
                {
                    Completed = true;
                }

                break;
            }
        }
    }
}