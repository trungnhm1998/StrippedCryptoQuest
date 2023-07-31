using CryptoQuest.System.Dialogue;
using CryptoQuest.System.Dialogue.Managers;
using UnityEngine;

namespace CryptoQuest.Character.DialogueProviders
{
    public class YarnDialogueProvider : DialogueProviderBehaviour
    {
        [SerializeField] private string _yarnNodeName;

        public override void ShowDialogue()
        {
            YarnSpinnerDialogueManager.PlayDialogueRequested?.Invoke(_yarnNodeName);
        }
    }
}