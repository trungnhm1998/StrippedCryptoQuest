using System;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    public abstract class QuestData<TDef> : QuestInfo where TDef : QuestSO
    {
        private const string DEBUG_QUEST_HEADER = "<size=18><color=red>[QUEST-SYSTEM]</color>";
        private const string DEBUG_QUEST_FOOTER = "</size>";

        [field: SerializeField] public TDef Data { get; protected set; }

        protected QuestData(TDef questDef) => Data = questDef;

        protected QuestData() { }
        public override QuestSO GetBaseData() => Data;

        public override void TriggerQuest()
        {
            Debug.Log(
                $"{DEBUG_QUEST_HEADER} Start Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color> {DEBUG_QUEST_FOOTER}");
        }

        public override void FinishQuest()
        {
            Debug.Log(
                $"{DEBUG_QUEST_HEADER} Finish Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color> {DEBUG_QUEST_FOOTER}");
            Data.OnQuestCompleted?.Invoke();

            if (Data.Rewards.Length <= 0) return;
            Data.OnRewardReceived?.Invoke(GetRewards());
        }
        private LootInfo[] GetRewards()
        {
            QuestReward[] rewards = Data.Rewards;
            return rewards.Select(reward => reward.CreateReward()).ToArray();
        }

        public override bool IsValid() => Data != null;
    }
}   