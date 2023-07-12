using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest.Dialogue.ScriptableObject;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs
{
    public class DialogueController : AbstractDialogController<UIDialogue>
    {
        [SerializeField] private DialogueEventChannelSO _dialogueEventSO;
        private DialogueScriptableObject _dialogueArgs;

        protected override void RegisterEvents()
        {
            _dialogueEventSO.ShowEvent += ShowDialog;
        }

        private void ShowDialog(DialogueScriptableObject dialogueArgs)
        {
            _dialogueArgs = dialogueArgs;
            LoadAssetDialog();
        }

        protected override void UnregisterEvents()
        {
            _dialogueEventSO.ShowEvent -= ShowDialog;
        }

        protected override void SetupDialog(UIDialogue dialog)
        {
            dialog
                .SetDialogue(_dialogueArgs)
                .Show();
        }
    }
}
