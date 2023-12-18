using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

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
            GenericMerchantDialogueController.Instance.InstantiateAsync(DialogInstantiated);
            ChoiceDialogController.Instance.InstantiateAsync(dialog => ChoiceDialog = dialog);
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