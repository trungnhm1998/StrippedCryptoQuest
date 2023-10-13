using System;
using CryptoQuest.Events.UI.Dialogs;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Dialogs.YesNoDialog
{
    public class YesNoDialogController : AbstractDialogController<UIYesNoDialog>
    {
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;

        private Action _yesPressed;
        private Action _noPressed;

        private UIYesNoDialog _uiYesNoDialog;
        private LocalizedString _message;

        protected override void RegisterEvents()
        {
            _yesNoDialogEventSO.ShowEvent += ShowDialogRequested;
            _yesNoDialogEventSO.HideEvent += HideDialogRequested;
            _yesNoDialogEventSO.SetMessageEvent += MessageReceived;
        }

        private void ShowDialogRequested(Action yesButtonPressed, Action noButtonPressed)
        {
            _yesPressed = yesButtonPressed;
            _noPressed = noButtonPressed;
            LoadAssetDialog();
        }

        private void MessageReceived(LocalizedString message)
        {
            _message = message;
        }

        private void HideDialogRequested()
        {
            _uiYesNoDialog?.Close();
        }

        protected override void UnregisterEvents()
        {
            _yesNoDialogEventSO.ShowEvent -= ShowDialogRequested;
            _yesNoDialogEventSO.HideEvent -= HideDialogRequested;
            _yesNoDialogEventSO.SetMessageEvent -= MessageReceived;
        }

        protected override void SetupDialog(UIYesNoDialog dialog)
        {
            _uiYesNoDialog = dialog;
            dialog
                .SetButtonsEvent(_yesPressed, _noPressed)
                .SetMessage(_message)
                .Show();
        }
    }
}
