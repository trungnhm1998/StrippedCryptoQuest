using CryptoQuest.Character.DialogueProviders;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class YarnQuestDialogueProvider : DialogueProviderBehaviour
    {
        [SerializeField] private YarnDialogWithQuestSo _yarnQuestNode;
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;

        public override void ShowDialogue()
        {
            if (_yarnQuestNode == null) return;
            GiveDialogueQuest();
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(_yarnQuestNode.YarnQuestDef.YarnNode);
        }

        public void SetYarnQuestNode(YarnDialogWithQuestSo yarnQuestNode) => _yarnQuestNode = yarnQuestNode;

        private void GiveDialogueQuest()
        {
            foreach (var possibleOutcome in _yarnQuestNode.YarnQuestDef.PossibleOutcomeQuests)
            {
                _giveQuestEventChannel.RaiseEvent(possibleOutcome);
            }
        }
    }
}