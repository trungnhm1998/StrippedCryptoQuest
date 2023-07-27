using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.Character.DialogueBehaviours
{
    public class ScriptableObjectDialogueController : MonoBehaviour, IDialogueController
    {
        [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;

        [Header("Raise on")]
        [SerializeField] private DialogueEventChannelSO _dialogueEventChannelSO;

        public void ShowDialogue()
        {
            _dialogueEventChannelSO.Show(_dialogueScriptableObject);
        }
    }
}