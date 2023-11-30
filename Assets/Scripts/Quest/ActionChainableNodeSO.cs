using System;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public abstract class ActionChainableNodeSO : ScriptableObject
    {
        public ActionChainableNodeSO nextActionChainableNode;

        public abstract void Execute();
        public static event Action Finished;

        protected virtual void ExecuteNextNode()
        {
            if (nextActionChainableNode == null)
            {
                Finished?.Invoke();
                return;
            }

            nextActionChainableNode.Execute();
        }
    }
}