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

        public void Configure(bool isQuestCompleted)
        {
            OnConfigure?.Invoke(isQuestCompleted);

            if (!isQuestCompleted) return;
            OnQuestCompletedHandler();
        }

        private void OnQuestCompletedHandler()
        {
            OnQuestCompleted?.Invoke();
        }

        public ActorSettingInfo CreateActorSettingInfo() => new(this);
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

        public void Configure(bool isQuestCompleted)
        {
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
            OnQuestCompleted?.Invoke();
        }
    }
}