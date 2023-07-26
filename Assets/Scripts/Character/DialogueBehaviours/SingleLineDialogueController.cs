using CryptoQuest.System.Dialogue.Events;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Character.DialogueBehaviours
{
    public class SingleLineDialogueController : MonoBehaviour, IDialogueController
    {
        [SerializeField] private LocalizedString _dialogueLine;

        [Header("Raise on")]
        [SerializeField] private PlayLocalizedLineEvent _playLocalizedLineEventEvent;

        public void ShowDialogue()
        {
            _playLocalizedLineEventEvent.RaiseEvent(_dialogueLine);
        }
    }
}