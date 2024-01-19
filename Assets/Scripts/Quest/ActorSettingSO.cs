using System;
using System.Collections.Generic;
using System.Linq;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Components;
using IndiGames.Core.Common;
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
        [field: SerializeField] public List<QuestSO> QuestsToTrack { get; set; } = new();
        public virtual void Configure(bool isQuestCompleted, string questsCompleted) {}
        public virtual void CompleteQuest(QuestSO questSo) {}
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
            _questCompletedDict = QuestsToTrack.ToDictionary(quest => quest.Guid, _ => false);
        }

        public void Configure(bool isQuestCompleted, string questsCompleted)
        {
            var questManager = ServiceProvider.GetService<IQuestManager>();

            questManager.OnQuestCompleted += CompleteQuest;

            OnConfigureQuestHandler();

            if (string.IsNullOrEmpty(questsCompleted) || !_questCompletedDict.ContainsKey(questsCompleted)) return;

            _questCompletedDict[questsCompleted] = isQuestCompleted;

            if ((QuestCondition != EConditionType.And || !_questCompletedDict.All(pair => pair.Value)) &&
                (QuestCondition == EConditionType.And || !_questCompletedDict.Any(pair => pair.Value))) return;

            OnQuestCompletedHandler();
        }

        public void CompleteQuest(QuestSO questSo)
        {
            if (!_questCompletedDict.ContainsKey(questSo.Guid)) return;

            _questCompletedDict[questSo.Guid] = true;

            if (!CheckCondition()) return;
            OnQuestCompletedHandler();
        }

        private void OnConfigureQuestHandler() => OnConfigure?.Invoke(CheckCondition());

        private void OnQuestCompletedHandler() => OnQuestCompleted?.Invoke();

        private bool CheckCondition()
        {
            return QuestCondition == EConditionType.And
                ? _questCompletedDict.All(pair => pair.Value)
                : _questCompletedDict.Any(pair => pair.Value);
        }
    }
}