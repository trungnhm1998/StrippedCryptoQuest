using CryptoQuest.Gameplay.Quest.Dialogue;

namespace CryptoQuest.UI.Dialogs.Dialogue
{
    public class DialogueController : AbstractDialogController<UIDialogue>
    {
        private IDialogueDef _dialogueArgs;

        protected override void RegisterEvents() { }
        protected override void UnregisterEvents() { }

        public void Show(IDialogueDef dialogueArgs)
        {
            ShowDialog(dialogueArgs);
        }

        private void ShowDialog(IDialogueDef dialogueArgs)
        {
            _dialogueArgs = dialogueArgs;
            LoadAssetDialog();
        }


        protected override void SetupDialog(UIDialogue dialog)
        {
            dialog
                .SetDialogue(_dialogueArgs)
                .Show();
        }
    }
}