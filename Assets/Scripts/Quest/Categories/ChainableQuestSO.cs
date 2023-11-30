using CryptoQuest.Quest.Authoring;
using UnityEngine;


namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Chainable QuestSO", fileName = "ChainableQuest")]
    public class ChainableQuestSO : QuestSO
    {
        public ActionChainableNodeSO actionChainableNode;

        public override QuestInfo CreateQuest()
            => new ChainableQuest(this);
    }

    public class ChainableQuest : QuestInfo<ChainableQuestSO>
    {
        public ChainableQuest(ChainableQuestSO questDef) : base(questDef) { }

        public override void GiveQuest()
        {
            base.GiveQuest();
            Data.actionChainableNode.Execute();
            ActionChainableNodeSO.Finished += InternalFinishedQuest;
            ActionChainableNodeSO.Failed += InternalFailedQuest;
        }

        private void InternalFinishedQuest()
        {
            ActionChainableNodeSO.Finished -= InternalFinishedQuest;
            FinishQuest();
        }

        private void InternalFailedQuest()
        {
            ActionChainableNodeSO.Failed -= InternalFailedQuest;
        }
    }
}