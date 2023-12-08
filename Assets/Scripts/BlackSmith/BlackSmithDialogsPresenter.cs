using CryptoQuest.Events.UI.Dialogs;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using CryptoQuest.UI.Dialogs.Dialogue;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithDialogsPresenter : MonoBehaviour
    {
        public event UnityAction ConfirmYesEvent;
        public event UnityAction ConfirmNoEvent;

        [SerializeField] private LocalizedString _message;
        private UIDialogueForGenericMerchant _dialogue;
        public UIDialogueForGenericMerchant Dialogue { get => _dialogue; }
        public UIChoiceDialog ChoiceDialog { get; private set; }
        private LocalizedString _confirmMessage;

        public void ShowOverviewDialog()
        {
            if (_dialogue != null)
            {
                DialogInstantiated(_dialogue);
                return;
            }

            GenericMerchantDialogueController.Instance.Instantiate(DialogInstantiated);
        }

        public void HideOverviewDialog()
        {
            _dialogue.Hide();
        }

        private void DialogInstantiated(UIDialogueForGenericMerchant dialog)
        {
            _dialogue = dialog;
            _dialogue
                .SetMessage(_message)
                .Show();
        }

        public void ShowConfirmDialog(LocalizedString confirmMessage)
        {
            Dialogue.Hide();
            _confirmMessage = confirmMessage;
            if (ChoiceDialog != null)
            {
                ChoiceDialogInstantiated(ChoiceDialog);
                return;
            }

            ChoiceDialogController.Instance.Instantiate(ChoiceDialogInstantiated, false);
        }

        private void ChoiceDialogInstantiated(UIChoiceDialog dialog)
        {
            ChoiceDialog = dialog;
            ChoiceDialog
                .WithNoCallback(NoButtonPressed)
                .WithYesCallback(YesButtonPressed)
                .SetMessage(_confirmMessage)
                .Show();
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

        public void HideConfirmDialog()
        {
            ChoiceDialog.Hide();
        }
    }
}