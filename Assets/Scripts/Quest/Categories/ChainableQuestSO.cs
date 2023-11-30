using CryptoQuest.Quest.Authoring;
using UnityEngine;
using UnityEngine.Serialization;


namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Chainable QuestSO", fileName = "ChainableQuest")]
    public class ChainableQuestSO : QuestSO
    {
        [field: SerializeField] public ActionChainableNodeSO NodeToComplete { get; private set; }
        private ChainableQuest _quest;

        public override QuestInfo CreateQuest()
            => new ChainableQuest(this);
    }

    public class ChainableQuest : QuestInfo<ChainableQuestSO>
    {
        public ChainableQuest(ChainableQuestSO questDef) : base(questDef) { }

        public override void GiveQuest()
        {
            base.GiveQuest();
            ActionChainableNodeSO.Finished += InternalFinishedQuest;
        }

        private void InternalFinishedQuest(ActionChainableNodeSO actionChainableNodeSo)
        {
            if (actionChainableNodeSo != Data.NodeToComplete) return;
            ActionChainableNodeSO.Finished -= InternalFinishedQuest;
            FinishQuest();
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            ActionChainableNodeSO.Finished -= InternalFinishedQuest;
        }
    }
}