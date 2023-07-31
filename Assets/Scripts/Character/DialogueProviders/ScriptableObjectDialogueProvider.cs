using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.Character.DialogueProviders
{
    public class ScriptableObjectDialogueProvider : DialogueProviderBehaviour
    {
        [SerializeField] private DialogueScriptableObject _dialogueScriptableObject;

        [Header("Raise on")]
        [SerializeField] private DialogueEventChannelSO _dialogueEventChannelSO;

        public override void ShowDialogue()
        {
            _dialogueEventChannelSO.Show(_dialogueScriptableObject);
        }
    }
}