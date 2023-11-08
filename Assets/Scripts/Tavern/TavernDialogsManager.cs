using System;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Tavern
{
    public class TavernDialogsManager : MonoBehaviour
    {
        public UIDialogueForGenericMerchant Dialogue { get; private set; }
        public UIChoiceDialog ChoiceDialog { get; private set; }

        [SerializeField] private LocalizedString _welcomeMessage;

        public void TavernOpened()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
            ChoiceDialogController.Instance.Instantiate(dialog => ChoiceDialog = dialog, false);
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            Dialogue = dialog;
            Dialogue
                .SetMessage(_welcomeMessage)
                .Show();
        }

        public void ShowDialogue()
        {
            Dialogue
                .SetMessage(_welcomeMessage)
                .Show();
        }

        public void ShowChoiceDialog(Action yes, Action no)
        {
            ChoiceDialog
                .SetButtonsEvent(yes, no)
                .SetMessage(_welcomeMessage)
                .Show();
        }

        public void HideDialogue()
        {
            GenericMerchantDialogueController.Instance.Release(Dialogue);
        }
    }
}