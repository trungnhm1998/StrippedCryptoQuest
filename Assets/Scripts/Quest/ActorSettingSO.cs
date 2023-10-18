using System;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest
{
    [CreateAssetMenu(fileName = "ActorSettingSO", menuName = "QuestSystem/Quests/ActorSetting/Actor Setting")]
    public class ActorSettingSO : ScriptableObject, IQuestConfigure
    {
        [field: SerializeReference] public QuestSO QuestToTrack { get; set; }
        public Action<bool> OnConfigure;

        public void Configure(bool isQuestCompleted)
        {
            OnConfigure?.Invoke(isQuestCompleted);
        }
    }
}