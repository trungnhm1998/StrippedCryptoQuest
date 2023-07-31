using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Character.DialogueProviders
{
    public class SingleLineDialogueProvider : DialogueProviderBehaviour
    {
        [SerializeField] private LocalizedString _dialogueLine;

        [Header("Raise on")]
        [SerializeField] private PlayLocalizedLineEvent _playLocalizedLineEventEvent;

        public override void ShowDialogue()
        {
            _playLocalizedLineEventEvent.RaiseEvent(_dialogueLine);
        }
    }
}