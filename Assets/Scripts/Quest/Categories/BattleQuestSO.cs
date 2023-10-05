using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(menuName = "Crypto Quest/Quest System/Battle QuestSO", fileName = "BattleQuestSO")]
    public class BattleQuestSO : QuestSO
    {
        [field: SerializeField] public Battlefield BattlefieldToLoad { get; private set; }
        [field: SerializeField] public QuestSO WinQuest { get; private set; }
        [field: SerializeField] public QuestSO LoseQuest { get; private set; }

        public override QuestInfo CreateQuest(QuestManager questManager)
            => new BattleQuestInfo(questManager, this);
    }

    public class BattleQuestInfo : QuestInfo<BattleQuestSO>
    {
        private readonly QuestBattleController _questBattleController;

        public BattleQuestInfo(QuestManager questManager, BattleQuestSO questDef) : base(questDef)
        {
            _questBattleController = questManager.GetComponent<QuestBattleController>();
        }

        public override void TriggerQuest() { }

        public override void GiveQuest()
        {
            _questBattleController.GiveQuest(this);
            _questBattleController.TriggerBattle(Data.BattlefieldToLoad);
        }
    }
}