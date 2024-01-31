using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch
{
    public class RanchDialogsController : MonoBehaviour
    {
        [SerializeField] private LocalizedString _message;
        public UIDialogueForGenericMerchant NormalDialogue { get; private set; }
        public UIChoiceDialog ChoiceDialog { get; private set; }

        public void RanchOpened()
        {
            GenericMerchantDialogueController.Instance.InstantiateAsync(DialogInstantiated);
            ChoiceDialogController.Instance.InstantiateAsync(dialog => ChoiceDialog = dialog);
        }

        private void OnDisable()
        {
            GenericMerchantDialogueController.Instance.Release(NormalDialogue);
            ChoiceDialogController.Instance.Release(ChoiceDialog);
        }

        private void ShowWelcomeDialog(LocalizedString message)
        {
            NormalDialogue
                .SetMessage(message)
                .Show();
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            NormalDialogue = dialog;
            ShowWelcomeDialog(_message);
        }
    }
}