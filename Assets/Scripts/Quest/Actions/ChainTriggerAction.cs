using System.Collections;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "QuestSystem/Actions/ChainTriggerAction")]
    public class ChainTriggerAction : NextAction
    {
        public ActionChainableNodeSO ActionChainableNode;

        public override IEnumerator Execute()
        {
            ActionChainableNode.Execute();
            yield return null;
        }
    }
}