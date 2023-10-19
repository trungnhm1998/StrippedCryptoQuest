using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Quest.Actions;
using CryptoQuest.Quest.Components;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CryptoQuest.Quest.Authoring
{
    [Serializable]
    public struct QuestReward
    {
        [SerializeReference] public LootInfo RewardItem;

        /// <returns> a cloned of loot config</returns>
        public LootInfo CreateReward() => RewardItem.Clone();
    }

    public abstract class QuestSO : SerializableScriptableObject
    {
        public Action OnQuestCompleted;
        public Action<List<LootInfo>> OnRewardReceived;
        [field: SerializeField] public string QuestID { get; private set; }
        [field: SerializeField] public string QuestName { get; private set; }
        [field: SerializeField] public string EventID { get; private set; }
        [field: SerializeField] public string EventName { get; private set; }
        [field: SerializeField, TextArea] public string EventDescription { get; private set; }

        [field: Header("Optional")]
        [field: SerializeReference] public NextAction NextAction { get; protected set; }

        [SerializeField] private QuestReward[] _rewards = Array.Empty<QuestReward>();
        public QuestReward[] Rewards => _rewards;
        public abstract QuestInfo CreateQuest(QuestManager questManager);

#if UNITY_EDITOR
        public void Editor_AddReward(LootInfo loot)
        {
            ArrayUtility.Add(ref _rewards, new QuestReward()
            {
                RewardItem = loot
            });
        }

        public void Editor_ClearReward()
        {
            _rewards = Array.Empty<QuestReward>();
        }
#endif
    }

    public abstract class QuestSO<T> : QuestSO where T : QuestInfo, new()
    {
        public override QuestInfo CreateQuest(QuestManager questManager) => new T();
    }
}