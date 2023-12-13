using System;
using CryptoQuest.UI.Dialogs.ChoiceDialog;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.Menus.DimensionalBox.UI.MetaDTransfer
{
    public class ConfirmTransferDialogController : MonoBehaviour
    {
        public event Action ConfirmYesEvent;
        public event Action ConfirmNoEvent;

        [SerializeField] private LocalizedString _confirmMessage;

        public UIChoiceDialog ChoiceDialog { get; private set; }

        public void ShowConfirmDialog()
        {
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
            ChoiceDialogController.Instance.Release(ChoiceDialog);
        }
    }
}