using System;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Quest.Components;
using IndiGames.Core.SaveSystem.ScriptableObjects;
using UnityEditor;
using UnityEngine;

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
        public Action<LootInfo[]> OnRewardReceived;
        public abstract QuestInfo CreateQuest(QuestManager questManager);
        [field: SerializeField] public string QuestID { get; private set; }
        [field: SerializeField] public string QuestName { get; private set; }

        [field: SerializeField] public string EventID { get; private set; }
        [field: SerializeField] public string EventName { get; private set; }
        [field: SerializeField, TextArea] public string EventDescription { get; private set; }

        [SerializeField] private QuestReward[] _rewards = Array.Empty<QuestReward>();
        public QuestReward[] Rewards => _rewards;

       
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