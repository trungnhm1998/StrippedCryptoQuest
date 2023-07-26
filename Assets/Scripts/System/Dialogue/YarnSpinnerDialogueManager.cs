using CryptoQuest.Gameplay.Cutscenes.Events;
using CryptoQuest.Input;
using CryptoQuest.System.Dialogue.Events;
using IndiGames.Core.Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue
{
    public class YarnSpinnerDialogueManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [Header("UI")]
        [SerializeField] private DialogueRunner _dialogueRunner;

        [Header("Listen to")]
        [SerializeField] private PlayDialogueEvent _playDialogueEventEvent;

        [Header("Raise on")]
        [SerializeField] private VoidEventChannelSO _dialogueCompletedEventChannelSO;
        [SerializeField] private PauseCutsceneEvent _pauseCutsceneEvent;

        [SerializeField] private UnityEvent _onDialogueCompleted;

        private Yarn.Dialogue Dialogue => _dialogueRunner.Dialogue;

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
            if (Dialogue.IsActive)
            {
                Debug.LogWarning(
                    "YarnSpinnerDialogueManager::ShowDialogue: Try run show dialogue while the previous still running.");
                _pauseCutsceneEvent.RaiseEvent(true);
                return;
            }

            Debug.Log($"YarnSpinnerDialogueManager::ShowDialogue: yarnNodeName[{yarnNodeName}]");
            _inputMediator.EnableDialogueInput();
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