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
        [field: SerializeField] public EConditionType QuestCondition { get; set; } = EConditionType.Required;

        [field: SerializeField]
        public List<QuestSO> QuestsToTrack { get; set; } = new();

        public virtual void Configure(bool isQuestCompleted, string questsCompleted) { }
        public virtual void CompleteQuest(string questGuid) { }

        public ActorSettingInfo CreateActorSettingInfo() => new(this);
    }

    public class ActorSettingInfo : IQuestConfigure
    {
        public Action<bool> OnConfigure;
        public Action OnQuestCompleted;
        public EConditionType QuestCondition { get; set; }
        public List<QuestSO> QuestsToTrack { get; set; }

        private readonly Dictionary<string, bool> _questCompletedDict;

        public ActorSettingInfo(ActorSettingSO data)
        {
            QuestCondition = data.QuestCondition;
            QuestsToTrack = data.QuestsToTrack;
            _questCompletedDict = QuestsToTrack.ToDictionary(quest => quest.Guid, quest => false);
        }

        public void Configure(bool isQuestCompleted, string questsCompleted)
        {
            OnConfigureQuestHandler();

            if (string.IsNullOrEmpty(questsCompleted) || !_questCompletedDict.ContainsKey(questsCompleted)) return;

            _questCompletedDict[questsCompleted] = isQuestCompleted;

            if ((QuestCondition != EConditionType.And || !_questCompletedDict.All(pair => pair.Value)) &&
                (QuestCondition == EConditionType.And || !_questCompletedDict.Any(pair => pair.Value))) return;

            OnQuestCompletedHandler();
        }

        public void CompleteQuest(string questGuid)
        {
            if (!_questCompletedDict.ContainsKey(questGuid)) return;

            _questCompletedDict[questGuid] = true;
            OnQuestCompletedHandler();
        }

        public void Subscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted += OnQuestCompletedHandler);

        public void Unsubscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted -= OnQuestCompletedHandler);

        private void OnConfigureQuestHandler()
        {
            bool conditionMet = QuestCondition == EConditionType.And
                ? _questCompletedDict.All(pair => pair.Value)
                : _questCompletedDict.Any(pair => pair.Value);

            OnConfigure?.Invoke(conditionMet);
        }

        private void OnQuestCompletedHandler()
        {
            OnQuestCompleted?.Invoke();
        }
    }
}