using CryptoQuest.Character.DialogueProviders;
using UnityEngine;

namespace CryptoQuest.Quest
{
    public class QuestDialogueProvider : DialogueProviderBehaviour
    {
        [SerializeField] private QuestProgressionConfigs _config;

        public override void ShowDialogue()
        {
            QuestDialogController.PlayQuestDialogue?.Invoke(_config);
        }
    }
}