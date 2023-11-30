using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace CryptoQuest.Quest
{
    public abstract class ActionChainableNodeSO : ScriptableObject
    {
        [field: SerializeField] public ActionChainableNodeSO NextActionChainableNode { get; protected set; }

        public abstract void Execute();
        public static event Action<ActionChainableNodeSO> Finished;

        protected virtual void ExecuteNextNode()
        {
            if (NextActionChainableNode == null)
            {
                Finished?.Invoke(this);
                return;
            }

            NextActionChainableNode.Execute();
        }
    }
}