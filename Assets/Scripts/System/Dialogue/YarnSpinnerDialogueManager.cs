using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using Yarn.Unity;

namespace CryptoQuest.System.Dialogue
{
    public class YarnSpinnerDialogueManager : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private DialogueRunner _dialogueRunner;

        [Header("Listen to")]
        [SerializeField] private PlayDialogueEvent _playDialogueEventEvent;

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
            Debug.Log($"DialogueManager: ShowDialogue: {yarnNodeName}");
        }
    }
}