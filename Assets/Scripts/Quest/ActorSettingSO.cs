using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public enum EConditionType
    {
        Required = 0,
        Or = 1,
        And = 2
    }

    [CreateAssetMenu(fileName = "ActorSettingSO", menuName = "QuestSystem/Quests/ActorSetting/Actor Setting")]
    public class ActorSettingSO : ScriptableObject, IQuestConfigure
    {
        public Action<bool> OnConfigure;
        public Action OnQuestCompleted;

        [field: SerializeField] public EConditionType QuestCondition { get; set; } = EConditionType.Required;

        [field: SerializeField]
        public List<QuestSO> QuestsToTrack { get; set; } = new();

        public virtual void Configure(bool isQuestCompleted, int questCompletedCount) { }
        public ActorSettingInfo CreateActorSettingInfo() => new(this);
    }

    public class ActorSettingInfo : IQuestConfigure
    {
        public Action<bool> OnConfigure;
        public Action OnQuestCompleted;
        private int _questCompletedCount;
        public EConditionType QuestCondition { get; set; }
        public List<QuestSO> QuestsToTrack { get; set; }

        public ActorSettingInfo(ActorSettingSO data)
        {
            QuestCondition = data.QuestCondition;
            QuestsToTrack = data.QuestsToTrack;
        }

        public void Configure(bool isQuestCompleted, int questCompletedCount)
        {
            _questCompletedCount = questCompletedCount;
            OnConfigure?.Invoke(isQuestCompleted);

            if (!isQuestCompleted) return;
            OnQuestCompletedHandler();
        }

        public void Subscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted += OnQuestCompletedHandler);

        public void Unsubscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted -= OnQuestCompletedHandler);

        private void OnQuestCompletedHandler()
        {
            _questCompletedCount++;
            switch (QuestCondition)
            {
                default:
                case EConditionType.Required:
                case EConditionType.Or:
                    OnQuestCompleted?.Invoke();
                    break;
                case EConditionType.And:
                    if (_questCompletedCount != QuestsToTrack.Count) return;
                    OnQuestCompleted?.Invoke();
                    break;
            }
        }
    }
}