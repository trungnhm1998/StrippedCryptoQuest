using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    /// <summary>
    /// This class is used to create a task
    /// that can be completed by completing any of the tasks in the list 
    /// </summary>
    [CreateAssetMenu(menuName = "Quest System/Or Task")]
    public class OrTasks : Task
    {
        public Task[] Tasks;

        public override bool EqualTo(Task other)
        {
            foreach (var objective in Tasks)
            {
                if (objective.EqualTo(other))
                {
                    return true;
                }
            }

            return base.EqualTo(other);
        }
    }
}