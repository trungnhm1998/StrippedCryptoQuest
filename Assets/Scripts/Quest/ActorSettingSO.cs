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

        public void Configure(bool isQuestCompleted) => OnConfigure?.Invoke(isQuestCompleted);
        public void Subscribe() => QuestsToTrack.ForEach(q => q.OnQuestCompleted += SetupQuest);
        public void Unsubscribe() => QuestsToTrack.ForEach(q => q.OnQuestCompleted -= SetupQuest);

        private void SetupQuest()
        {
            QuestsToTrack.ForEach(q => q.OnQuestCompleted -= SetupQuest);
            OnQuestCompleted?.Invoke();
        }
    }
}