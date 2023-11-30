using System;
using CryptoQuest.Quest.Authoring;
using UnityEngine;


namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Chainable QuestSO", fileName = "ChainableQuest")]
    public class ChainableQuestSO : QuestSO
    {
        public ActionChainableNodeSO actionChainableNode;
        private ChainableQuest _quest;

        public override QuestInfo CreateQuest()
            => new ChainableQuest(this);
    }

    public class ChainableQuest : QuestInfo<ChainableQuestSO>, IDisposable
    {
        public ChainableQuest(ChainableQuestSO questDef) : base(questDef) { }

        public override void GiveQuest()
        {
            base.GiveQuest();
            ActionChainableNodeSO.Finished += InternalFinishedQuest;
        }

        private void InternalFinishedQuest()
        {
            ActionChainableNodeSO.Finished -= InternalFinishedQuest;
            FinishQuest();
        }

        public void Dispose()
        {
            ActionChainableNodeSO.Finished -= InternalFinishedQuest;
        }
    }
}