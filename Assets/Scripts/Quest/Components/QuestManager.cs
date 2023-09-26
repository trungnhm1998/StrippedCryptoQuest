using System;
using System.Collections.Generic;
using CryptoQuest.Quest.Authoring;
using IndiGames.Core.EditorTools.Attributes.ReadOnlyAttribute;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    [AddComponentMenu("Quest System/Quest Manager")]
    [DisallowMultipleComponent]
    public class QuestManager : MonoBehaviour
    {
        public static Action<QuestSO> OnTriggerQuest;

        [field: SerializeField, ReadOnly] public List<string> InProgressQuests { get; private set; } = new();
        [field: SerializeField, ReadOnly] public List<string> CompletedQuests { get; private set; } = new();
        [field: ReadOnly] public List<Authoring.Quest> Quests { get; private set; } = new();

        private void OnEnable()
        {
            OnTriggerQuest += TriggerQuest;
        }

        private void OnDisable()
        {
            OnTriggerQuest -= TriggerQuest;
        }

        private void TriggerQuest(QuestSO questDef)
        {
            var currentQuest = questDef.CreateQuest(this);
            currentQuest.TriggerQuest();

            Quests.Add(currentQuest);
            InProgressQuests.Add(questDef.Guid);
        }
    }
}