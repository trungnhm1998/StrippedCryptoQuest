using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.System.Dialogue.Managers;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;

namespace CryptoQuest.Quest.Components
{
    public class QuestDialogController : MonoBehaviour
    {
        [SerializeField] private PlayDialogueEvent _playDialogueEventChannel;

        [SerializeField] private VoidEventChannelSO _dialogCompletedEvent;

        private void OnEnable()
        {
            _playDialogueEventChannel.PlayDialogueRequested += ShowQuestDialogue;
            _dialogCompletedEvent.EventRaised += DialogCompleted;
        }

        private void OnDisable()
        {
            _playDialogueEventChannel.PlayDialogueRequested -= ShowQuestDialogue;
            _dialogCompletedEvent.EventRaised -= DialogCompleted;
        }

        private void ShowQuestDialogue(string yarnNode)
        {
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(yarnNode);
        }

        private void DialogCompleted()
        {
            Debug.Log("Dialog completed");
        }
    }
}