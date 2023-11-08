using CryptoQuest.UI.Dialogs.Dialogue;
using CryptoQuest.Events.UI.Dialogs;
using UnityEngine.Localization;
using UnityEngine.Events;
using UnityEngine;
using CryptoQuest.UI.Dialogs.ChoiceDialog;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassDialogController : MonoBehaviour
    {
        public event UnityAction ConfirmYesEvent;
        public event UnityAction ConfirmNoEvent;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private LocalizedString _defaultMessage;
        public UIDialogueForGenericMerchant Dialogue { get; private set; }
        public UIChoiceDialog ChoiceDialog { get; private set; }

        public void ShowChangeClassDialog()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
            ChoiceDialogController.Instance.Instantiate(dialog => ChoiceDialog = dialog, false);
        }

        public void HideChangeClassDialog()
        {
            GenericMerchantDialogueController.Instance.Release(Dialogue);
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            Dialogue = dialog;
            Dialogue
                .SetMessage(_defaultMessage)
                .Show();
        }
    }
}
