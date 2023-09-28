using System;
using System.Collections.Generic;
using CryptoQuest.Gameplay.Loot;
using CryptoQuest.Gameplay.Reward;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        public static Action<IQuestConfigure> OnConfigureQuest;

        [SerializeField] private QuestTriggerEventChannelSO questTriggerEventChannel;
        [field: SerializeField, ReadOnly] public List<string> InProgressQuests { get; private set; } = new();
        [field: SerializeField, ReadOnly] public List<string> CompletedQuests { get; private set; } = new();
        [field: ReadOnly] public List<QuestInfo> Quests { get; private set; } = new();

        private QuestSO _currentQuest;
        private QuestInfo _currentQuestInfo;

        private void OnEnable()
        {
            OnConfigureQuest += ConfigureQuestHolder;
            questTriggerEventChannel.EventRaised += TriggerQuest;
        }

        private void OnDisable()
        {
            OnConfigureQuest -= ConfigureQuestHolder;
            questTriggerEventChannel.EventRaised -= TriggerQuest;
        }

        private void TriggerQuest(QuestSO questDef)
        {
            _currentQuest = questDef;

            if (InProgressQuests.Contains(_currentQuest.Guid) || CompletedQuests.Contains(_currentQuest.Guid)) return;

            var currentQuest = _currentQuest.CreateQuest(this);
            currentQuest.TriggerQuest();

            Quests.Add(currentQuest);
            InProgressQuests.Add(_currentQuest.Guid);

            _currentQuest.OnRewardReceived += RewardReceived;
            _currentQuest.OnQuestCompleted += QuestCompleted;
        }

        private void RewardReceived(LootInfo[] loots)
        {
            _currentQuest.OnRewardReceived -= RewardReceived;
        }

        private void QuestCompleted()
        {
            CompletedQuests.Add(_currentQuest.Guid);
            InProgressQuests.Remove(_currentQuest.Guid);

            _currentQuest.OnQuestCompleted -= QuestCompleted;
        }

        private bool IsQuestTriggered(QuestSO questSo)
        {
            foreach (var quest in Quests)
            {
                if (quest.GetBaseData() == questSo)
                    return true;
            }

            return false;
        }

        private void ConfigureQuestHolder(IQuestConfigure questConfigure)
        {
            questConfigure.IsQuestCompleted = IsQuestTriggered(questConfigure.Quest);
            questConfigure.Configure();
        }
    }
}