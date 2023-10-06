using System.Collections;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "Quest/Actions/DialogueAction")]
    public class DialogueAction : NextAction
    {
        [SerializeField] private YarnDialogWithQuestSo _yarnQuestNode;
        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        public float Delay = 0.5f;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);
            if (_yarnQuestNode == null) yield break;
            GiveDialogueQuest();
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(_yarnQuestNode.YarnQuestDef.YarnNode);
        }

        private void GiveDialogueQuest()
        {
            foreach (var possibleOutcome in _yarnQuestNode.YarnQuestDef.PossibleOutcomeQuests)
            {
                _giveQuestEventChannel.RaiseEvent(possibleOutcome);
            }
        }
    }
}