using CryptoQuest.System.Dialogue.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue
{
    public class YarnSpinnerDialogueManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private DialogueRunner _dialogueRunner;

        [Header("Listen to")]
        [SerializeField] private PlayDialogueEvent _playDialogueEventEvent;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _dialogueCompletedEventChannelSO;

        [SerializeField] private UnityEvent _onDialogueCompleted;

        private void OnEnable()
        {
            _playDialogueEventEvent.PlayDialogueRequested += ShowDialogue;
        }

        private void OnDisable()
        {
            _playDialogueEventEvent.PlayDialogueRequested -= ShowDialogue;
        }

        private void ShowDialogue(string yarnNodeName)
        {
            Debug.Log($"YarnSpinnerDialogueManager::ShowDialogue: yarnNodeName[{yarnNodeName}]");
            _dialogueRunner.StartDialogue(yarnNodeName);
        }

        public void DialogueCompleted()
        {
             // support cross scene
            if (_dialogueCompletedEventChannelSO != null) _dialogueCompletedEventChannelSO.RaiseEvent();
            _onDialogueCompleted.Invoke();
        }
    }
}