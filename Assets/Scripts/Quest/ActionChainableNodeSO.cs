using System;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public abstract class ActionChainableNodeSO : ScriptableObject
    {
        public ActionChainableNodeSO nextActionChainableNode;

        public abstract void Execute();
        public static event Action Finished;
        public static event Action Failed;

        protected virtual void ExecuteNextNode()
        {
            if (nextActionChainableNode == null)
            {
                Finished?.Invoke();
                return;
            }

            nextActionChainableNode.Execute();
        }

        protected void OnQuestFailed()
        {
            Failed?.Invoke();
        }
    }
}