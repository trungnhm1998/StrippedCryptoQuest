using System;
using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(fileName = "ActorSettingSO", menuName = "QuestSystem/Quests/ActorSetting/Actor Setting")]
    public class ActorSettingSO : ScriptableObject, IQuestConfigure
    {
        public Action<bool> OnConfigure;
        public Action OnQuestCompleted;
        [field: SerializeReference] public List<QuestSO> QuestsToTrack { get; set; } = new();

        private bool _questCompletedSubscribed = true;

        public void Configure(bool isQuestCompleted) => OnConfigure?.Invoke(isQuestCompleted);

        public void Subscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted += OnQuestCompletedHandler);

        public void Unsubscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted -= OnQuestCompletedHandler);

        private void OnQuestCompletedHandler()
        {
            if (!_questCompletedSubscribed) return;

            OnQuestCompleted?.Invoke();
            _questCompletedSubscribed = false;
        }

        public ActorSettingInfo CreateActorSettingInfo() =>
            new ActorSettingInfo(this);
    }

    public class ActorSettingInfo : IQuestConfigure
    {
        public Action<bool> OnConfigure;
        public Action OnQuestCompleted;
        public List<QuestSO> QuestsToTrack { get; set; } = new();

        public ActorSettingInfo(ActorSettingSO data)
        {
            QuestsToTrack = data.QuestsToTrack;
        }

        public void Configure(bool isQuestCompleted) => OnConfigure?.Invoke(isQuestCompleted);

        public void Subscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted += OnQuestCompletedHandler);

        public void Unsubscribe() =>
            QuestsToTrack.ForEach(questData => questData.OnQuestCompleted -= OnQuestCompletedHandler);

        private void OnQuestCompletedHandler()
        {
            OnQuestCompleted?.Invoke();
        }
    }
}