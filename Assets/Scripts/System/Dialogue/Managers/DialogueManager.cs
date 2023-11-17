using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.Gameplay.Quest.Dialogue;
using CryptoQuest.Input;
using CryptoQuest.System.Dialogue.Builder;
using CryptoQuest.System.Dialogue.Events;
using CryptoQuest.UI.Dialogs.Dialogue;
using Input;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.System.Dialogue.Managers
{
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField] private InputMediatorSO _inputMediator;
        [SerializeField] private DialogueController _gameDialogueController;

        [Header("Listen to")]
        [SerializeField] private PlayLocalizedLineEvent _playLocalizedLineEventEvent;

        /// <summary>
        /// Before YarnSpinner we already had a dialogue system. This is the old event channel.
        /// </summary>
        [SerializeField] private DialogueEventChannelSO _oldDialogueEventChannelSO;

        private void OnEnable()
        {
            _playLocalizedLineEventEvent.PlayDialogueRequested += ShowDialogue;
            _oldDialogueEventChannelSO.ShowEvent += ShowDialogue;
        }

        private void OnDisable()
        {
            _playLocalizedLineEventEvent.PlayDialogueRequested -= ShowDialogue;
            _oldDialogueEventChannelSO.ShowEvent -= ShowDialogue;
        }

        /// <summary>
        /// This method will use the <see cref="DialogueController"/> to show the dialogue.
        /// </summary>
        /// <param name="localizedLine"></param>
        private void ShowDialogue(LocalizedString localizedLine)
        {
            Dialogue dialogue = A.Dialogue.WithLines(localizedLine);
            ShowDialogue(dialogue);
        }

        private void ShowDialogue(IDialogueDef dialogue)
        {
            _inputMediator.EnableDialogueInput();
            _gameDialogueController.Show(dialogue);
        }
    }
}