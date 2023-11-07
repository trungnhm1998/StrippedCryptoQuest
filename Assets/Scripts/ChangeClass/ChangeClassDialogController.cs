using CryptoQuest.UI.Dialogs.Dialogue;
using CryptoQuest.Events.UI.Dialogs;
using UnityEngine.Localization;
using UnityEngine.Events;
using UnityEngine;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassDialogController : MonoBehaviour
    {
        public event UnityAction ConfirmYesEvent;
        public event UnityAction ConfirmNoEvent;
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        [SerializeField] private LocalizedString _defaultMessage;
        public UIDialogueForGenericMerchant Dialogue { get; private set; }

        public void ShowChangeClassDialog()
        {
            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
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

        public void ShowConfirmDialog(LocalizedString confirmMessage)
        {
            _yesNoDialogEventSO.SetMessage(confirmMessage);
            _yesNoDialogEventSO.Show(YesButtonPressed, NoButtonPressed);
        }

        private void YesButtonPressed()
        {
            ConfirmYesEvent?.Invoke();
            HideConfirmDialog();
        }

        private void NoButtonPressed()
        {
            ConfirmNoEvent?.Invoke();
            HideConfirmDialog();
        }

        private void HideConfirmDialog()
        {
            _yesNoDialogEventSO.Hide();
        }
    }
}
