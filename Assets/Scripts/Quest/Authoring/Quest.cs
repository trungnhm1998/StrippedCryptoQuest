using System;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Quest System/Quest")]
    public class Quest : AbstractObjective
    {
        /// <summary>
        /// Only check objectives if this is false..
        /// </summary>
        [Header("Details")]
        [SerializeField] private int _currentTaskIndex;

        [SerializeField] private Task[] _tasks = Array.Empty<Task>();
        public Task[] Tasks => _tasks;


        public Task GetCurrentTask()
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var nextTask = _tasks[index];
                if (!nextTask.IsCompleted)
                {
                    _currentTaskIndex = index;
                    return nextTask;
                }
            }

            return null;
        }

        public bool HasQuestCompleted(Task task)
        {
            if (IsCompleted)
            {
                Debug.LogWarning($"Quest: <color=red>{name}</color> is completed, next task is " +
                                 $"<color=yellow>{task.name}</color> .");
                return false;
            }

            if (!CanCompleteTask(task))
            {
                Debug.LogWarning($"Task: <color=red>{task.name}</color> was completed.");
                return false;
            }

            return true;
        }

        private bool CanCompleteTask(Task task)
        {
            foreach (var currentTask in _tasks)
            {
                if (!currentTask.EqualTo(task)) continue;
                if (currentTask.IsCompleted) return false;
            }

            return true;
        }

        public void CompleteTask(Task task)
        {
            foreach (var configTask in _tasks)
            {
                if (!configTask.EqualTo(task)) continue;
                task.OnComplete();
            }
        }

        public override void OnComplete()
        {
            base.OnComplete();
            Debug.Log($"Quest: <color=green>{name}</color> is completed.");
        }

        public override void OnProgressChange()
        {
            if (Array.TrueForAll(_tasks, task => task.IsCompleted))
            {
                OnComplete();
            }
        }

        public override void SubscribeObjective()
        {
            foreach (var task in _tasks)
            {
                task.OnCompleteObjective += OnProgressChange;
            }
        }

        public override void UnsubscribeObjective()
        {
            foreach (var task in _tasks)
            {
                task.OnCompleteObjective -= OnProgressChange;
            }
        }
    }
}