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

        public bool HasTaskCompleted(Task task)
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var configTask = _tasks[index];
                if (configTask.CompareTo(task) != 0) continue;
                return IsCompleted;
            }

            return false;
        }

        public bool CanCompleteTask(Task task)
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var configTask = _tasks[index];
                if (configTask.CompareTo(task) != 0) continue;

                if (index <= 0) continue;

                if (!task.IsCompleted) return false;
            }

            return true;
        }

        public void CompleteTask(Task task)
        {
            for (var index = 0; index < _tasks.Length; index++)
            {
                var configTask = _tasks[index];
                if (configTask.CompareTo(task) != 0) continue;

                if (index == _tasks.Length - 1)
                {
                    task.OnComplete();
                }

                break;
            }
        }

        public override void OnComplete()
        {
            IsCompleted = true;
            OnCompleteObjective?.Invoke();
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