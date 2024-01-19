using System;
using CryptoQuest.Quest.Components;
using IndiGames.Core.Common;
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
            Debug.Log($"<color=green>QuestSystem:</color>" +
                      $":Start Quest: Chapter: [{Data.Chapter}] -  [{Data.QuestName}]");
        }

        public override void FinishQuest()
        {
            Debug.Log($"<color=green>QuestSystem:</color>" +
                      $":Finish Quest: Chapter: [{Data.Chapter}] -[{Data.QuestName}] ");
            var questManager = ServiceProvider.GetService<IQuestManager>();

            Data.OnQuestCompleted?.Invoke();
            questManager?.OnQuestCompleted?.Invoke(Data);
        }

        public override void GiveQuest()
        {
            Debug.Log($"<color=green>QuestSystem:</color>" +
                      $":Give Quest: Chapter: [{Data.Chapter}] -[{Data.QuestName}] ");
        }

        public override bool IsValid() => Data != null;
    }
}