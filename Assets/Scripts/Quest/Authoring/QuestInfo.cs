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
            Debug.Log($"QuestSystem::Start Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color>");
        }

        public override void FinishQuest()
        {
            Debug.Log($"QuestSystem::Finish Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color>");
            Data.OnQuestCompleted?.Invoke();
            QuestManager.OnQuestCompleted?.Invoke(Data);

            if (Data.Rewards.Length > 0)
            {
                Data.OnRewardReceived?.Invoke(GetRewards());
            }

            if (Data.NextAction == null) return;
            _questManager.StartCoroutine(Data.NextAction.Execute());
        }

        public override void GiveQuest()
        {
            Debug.Log(
                $"QuestSystem::Give Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color>");
        }

        private LootInfo[] GetRewards()
        {
            QuestReward[] rewards = Data.Rewards;
            return rewards.Select(reward => reward.CreateReward()).ToArray();
        }

        public override bool IsValid() => Data != null;
    }
}