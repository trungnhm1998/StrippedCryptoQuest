using CryptoQuest.Battle;
using CryptoQuest.Battle.Events;
using CryptoQuest.Gameplay.Encounter;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using CryptoQuest.Quest.Controller;
using CryptoQuest.Quest.Controllers;
using CryptoQuest.System.CutsceneSystem;
using CryptoQuest.System.CutsceneSystem.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace CryptoQuest.Quest.Categories
{
    [CreateAssetMenu(menuName = "QuestSystem/Quests/Cutscene Battle QuestSO", fileName = "CutsceneBattleQuestSO")]
    public class CutsceneBattleQuestSO : QuestSO
    {
        [field: SerializeField] public BattleResultEventSO BattleWinResult { get; private set; }
        [field: SerializeField] public PlayableAsset CutsceneDefToTrack { get; private set; }
        [field: SerializeField] public Battlefield BattleToTrack { get; private set; }

        public override QuestInfo CreateQuest(QuestManager questManager)
            => new CutsceneBattleQuestInfo(this);
    }

    public class CutsceneBattleQuestInfo : QuestInfo<CutsceneBattleQuestSO>
    {
        public CutsceneBattleQuestInfo(CutsceneBattleQuestSO questDef) : base(questDef) { }

        public override void TriggerQuest()
        {
            base.TriggerQuest();
            FinishQuest();
        }

        public override void GiveQuest()
        {
            base.GiveQuest();
            CutsceneManager.CutsceneFinished += HandleCutsceneEnd;
        }

        private void HandleCutsceneEnd(PlayableDirector director)
        {
            if (director.playableAsset != Data.CutsceneDefToTrack) return;


            BattleLoader.RequestLoadBattle(Data.BattleToTrack);
            Data.BattleWinResult.EventRaised += HandleBattleEnd;
        }

        private void HandleBattleEnd(Battlefield battlefield)
        {
            if (battlefield != Data.BattleToTrack) return;
            Data.BattleWinResult.EventRaised -= HandleBattleEnd;
            CutsceneManager.CutsceneFinished -= HandleCutsceneEnd;
            FinishQuest();
        }
    }
}