using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Quest.Components;
using CryptoQuest.System;
using IndiGames.Core.Common;
using Newtonsoft.Json;
using UnityEngine;

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    public abstract class QuestInfo
    {
        public string Guid => BaseData.Guid;
        public abstract QuestSO BaseData { get; }
        public abstract void TriggerQuest();
        public abstract void FinishQuest();
        public abstract bool IsValid();
        public abstract void GiveQuest();

        public void Release()
        {
            OnRelease();
        }

        protected virtual void OnRelease() { }
    }

    [Serializable]
    public abstract class QuestInfo<TDef> : QuestInfo where TDef : QuestSO
    {
        [SerializeField] private TDef _data;
        public TDef Data => _data;
        protected QuestInfo(TDef questDef) => _data = questDef;
        public override QuestSO BaseData => Data;
        protected QuestInfo() => _data = default(TDef);

        public override void TriggerQuest()
        {
            Debug.Log($"QuestSystem::Start Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color>");
        }

        public override void FinishQuest()
        {
            Debug.Log($"QuestSystem::Finish Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color>");
            var questManager = ServiceProvider.GetService<IQuestManager>();

            Data.OnQuestCompleted?.Invoke();
            questManager?.OnQuestCompleted?.Invoke(Data);

            if (Data.Rewards.Length > 0)
            {
                Data.OnRewardReceived?.Invoke(GetRewards());
            }

            if (Data.NextAction == null) return;
            questManager?.StartCoroutine(Data.NextAction.Execute());
        }

        public override void GiveQuest()
        {
            Debug.Log($"QuestSystem::Give Quest: <color=green>[{Data.QuestName}] - [{Data.EventName}]</color>");
        }

        private List<LootInfo> GetRewards()
        {
            QuestReward[] rewards = Data.Rewards;
            return rewards.Select(reward => reward.CreateReward()).ToList();
        }

        public override bool IsValid() => Data != null;
    }
}