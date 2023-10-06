using System;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Quest.Components;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    public abstract class QuestInfo
    {
        public abstract QuestSO BaseData { get; }
        public abstract void TriggerQuest();
        public abstract void FinishQuest();
        public abstract bool IsValid();
        public abstract void GiveQuest();
    }

    [Serializable]
    public abstract class QuestInfo<TDef> : QuestInfo where TDef : QuestSO
    {
        private const string DEBUG_QUEST_HEADER = "<size=18><color=red>[QUEST-SYSTEM]</color>";
        private const string DEBUG_QUEST_FOOTER = "</size>";
        protected QuestManager _questManager { get; private set; }
        [field: SerializeField] public TDef Data { get; protected set; }

        protected QuestInfo(TDef questDef) => Data = questDef;

        protected QuestInfo(QuestManager questManager, TDef questDef)
        {
            _questManager = questManager;
            Data = questDef;
        }

        public override QuestSO BaseData => Data;

        protected QuestInfo()
        {
        }

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
            if (Data.NextAction != null)
                _questManager.StartCoroutine(Data.NextAction.Execute());
            if (Data.Rewards.Length <= 0) return;
            Data.OnRewardReceived?.Invoke(GetRewards());
        }

        public override void GiveQuest()
        {
            Debug.Log(
                $"{DEBUG_QUEST_HEADER} Give Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color> {DEBUG_QUEST_FOOTER}");
        }

        private LootInfo[] GetRewards()
        {
            QuestReward[] rewards = Data.Rewards;
            return rewards.Select(reward => reward.CreateReward()).ToArray();
        }

        public override bool IsValid() => Data != null;
    }
}