using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Ranch
{
    public class RanchDialogsManager : MonoBehaviour
    {
        [SerializeField] private LocalizedString _message;
        public UIDialogueForGenericMerchant NormalDialogue { get; private set; }

        public void RanchOpened()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
        }

        public void RanchClosed()
        {
            GenericMerchantDialogueController.Instance.Release(NormalDialogue);
        }

        public void ShowWelcomeDialog()
        {
            NormalDialogue
                .SetMessage(_message)
                .Show();
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            NormalDialogue = dialog;
            ShowWelcomeDialog();
        }
    }
}