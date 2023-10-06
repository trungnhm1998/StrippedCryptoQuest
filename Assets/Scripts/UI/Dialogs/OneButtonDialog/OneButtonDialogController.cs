using CryptoQuest.Events.UI.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

namespace CryptoQuest.UI.Dialogs.OneButtonDialog
{
    public class OneButtonDialogController : AbstractDialogController<UIOneButtonDialog>
    {
        [SerializeField] private OneButtonDialogEventChannelSO _oneButtonDialogEventSO;

        private Action _buttonPressed;

        private UIOneButtonDialog _uiOneButtonDialog;
        private LocalizedString _message;
        private LocalizedString _buttonMessage;

        protected override void RegisterEvents()
        {
            _oneButtonDialogEventSO.ShowEvent += ShowDialogRequested;
            _oneButtonDialogEventSO.HideEvent += HideDialogRequested;
            _oneButtonDialogEventSO.SetMessageEvent += MessageReceived;
            _oneButtonDialogEventSO.SetButtonTextEvent += ButtonTextReceived;
        }

        private void ShowDialogRequested(Action buttonPressed)
        {
            _buttonPressed = buttonPressed;
            LoadAssetDialog();
        }

        private void MessageReceived(LocalizedString message)
        {
            _message = message;
        }

        private void ButtonTextReceived(LocalizedString message)
        {
            _buttonMessage = message;
        }

        private void HideDialogRequested()
        {
            _uiOneButtonDialog.Close();
        }

        protected override void UnregisterEvents()
        {
            _oneButtonDialogEventSO.ShowEvent -= ShowDialogRequested;
            _oneButtonDialogEventSO.HideEvent -= HideDialogRequested;
            _oneButtonDialogEventSO.SetMessageEvent -= MessageReceived;
            _oneButtonDialogEventSO.SetButtonTextEvent -= ButtonTextReceived;
        }

        protected override void SetupDialog(UIOneButtonDialog dialog)
        {
            _uiOneButtonDialog = dialog;
            dialog
                .SetButtonsEvent(_buttonPressed)
                .SetMessage(_message)
                .SetButtonText(_buttonMessage)
                .Show();
        }
    }
}
