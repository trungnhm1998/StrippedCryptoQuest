using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controllers;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Battle QuestSO", fileName = "BattleQuestSO")]
    public class BattleQuestSO : QuestSO
    {
        [field: SerializeField] public Battlefield BattlefieldToConquer { get; private set; }
        [field: SerializeField] public QuestSO FirstTimeLoseQuest { get; private set; }
        [field: SerializeField] public QuestSO GiveRepeatBattleQuest { get; private set; }

        public override QuestInfo CreateQuest(QuestManager questManager)
            => new BattleQuestInfo(questManager, this);
    }

    public class BattleQuestInfo : QuestInfo<BattleQuestSO>
    {
        private readonly QuestBattleController _questBattleController;

        public BattleQuestInfo(QuestManager questManager, BattleQuestSO questDef) : base(questManager, questDef)
        {
            _questBattleController = questManager.GetComponent<QuestBattleController>();
        }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            _questBattleController.GiveQuest(this);
        }
    }
}