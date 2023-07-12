using System;
using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest;
using UnityEngine;
using UnityEngine.Events;

namespace CryptoQuest.UI.Dialogs
{
    public class YesNoDialogController : AbstractDialogController<UIYesNoDialog>
    {
        [SerializeField] private YesNoDialogEventChannelSO _yesNoDialogEventSO;
        private Action _yesPressed;
        private Action _noPressed;

        protected override void RegisterEvents()
        {
            _yesNoDialogEventSO.ShowEvent += ShowDialogRequested;
        }

        private void ShowDialogRequested(Action yesButtonPressed, Action noButtonPressed)
        {
            _yesPressed = yesButtonPressed;
            _noPressed = noButtonPressed;
            LoadAssetDialog();
        }


        protected override void UnregisterEvents()
        {
            _yesNoDialogEventSO.ShowEvent -= ShowDialogRequested;
        }

        protected override void SetupDialog(UIYesNoDialog dialog)
        {
            dialog.YesPressed = _yesPressed;
            dialog.NoPressed = _noPressed;
            dialog.Show();
        }
    }
}
