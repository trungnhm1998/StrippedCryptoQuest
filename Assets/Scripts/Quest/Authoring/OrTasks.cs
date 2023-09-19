using UnityEngine;

namespace CryptoQuest.Quest
{
    /// <summary>
    /// This class is used to create a task
    /// that can be completed by completing any of the tasks in the list 
    /// </summary>
    [CreateAssetMenu(menuName = "Quest System/Or Task")]
    public class OrTasks : Task
    {
        public Task[] Tasks;

        public override int CompareTo(Task other)
        {
            foreach (var objective in Tasks)
            {
                if (objective.CompareTo(other) == 0)
                {
                    return 0;
                }
            }
            return base.CompareTo(other);
        }
    }
}