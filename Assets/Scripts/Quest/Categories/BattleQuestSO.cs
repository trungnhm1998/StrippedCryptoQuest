using CryptoQuest.System;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Controllers;
using CryptoQuest.Quest.Components;
using IndiGames.Core.Common;
using UnityEngine;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Battle QuestSO", fileName = "BattleQuestSO")]
    public class BattleQuestSO : QuestSO
    {
        [field: SerializeField] public Battlefield BattlefieldToConquer { get; private set; }
        [field: SerializeField] public QuestSO FirstTimeLoseQuest { get; private set; }
        [field: SerializeField] public QuestSO GiveRepeatBattleQuest { get; private set; }

        public override QuestInfo CreateQuest()
            => new BattleQuestInfo(this);
    }

    public class BattleQuestInfo : QuestInfo<BattleQuestSO>
    {
        public BattleQuestInfo(BattleQuestSO questDef) : base(questDef) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            var questManager = ServiceProvider.GetService<IQuestManager>();
            var questBattleController = questManager?.GetComponent<QuestBattleController>();
            questBattleController?.GiveQuest(this);
        }
    }
}