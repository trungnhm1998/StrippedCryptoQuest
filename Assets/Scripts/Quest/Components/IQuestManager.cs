using System;
using CryptoQuest.Quest.Authoring;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public abstract class IQuestManager : MonoBehaviour
    {
        public static Action<IQuestConfigure> OnConfigureQuest;
        public static Action<QuestSO> OnRemoveProgressingQuest;
        public Action<QuestSO> OnQuestCompleted;

        public abstract void TriggerQuest(QuestSO questData);
        public abstract void GiveQuest(QuestSO questData);
    }
}