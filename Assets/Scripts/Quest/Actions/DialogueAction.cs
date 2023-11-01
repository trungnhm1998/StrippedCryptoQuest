using System.Collections;
using CryptoQuest.Quest.Authoring;
using CryptoQuest.Quest.Events;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;

namespace CryptoQuest.Quest.Actions
{
    [CreateAssetMenu(menuName = "QuestSystem/Actions/DialogueAction")]
    public class DialogueAction : NextAction
    {
        [Header("Default Events"), SerializeField]
        private YarnProjectConfigEvent _yarnProjectConfigEvent;

        [SerializeField] private QuestEventChannelSO _giveQuestEventChannel;
        public float Delay = 0.5f;

        [Header("Config for Quest")]
        [SerializeField] private YarnDialogWithQuestSo _yarnQuestNode;

        public override IEnumerator Execute()
        {
            yield return new WaitForSeconds(Delay);
            if (_yarnQuestNode == null) yield break;
            _yarnProjectConfigEvent.RaiseEvent(_yarnQuestNode.YarnQuestDef.YarnProjectConfig);
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