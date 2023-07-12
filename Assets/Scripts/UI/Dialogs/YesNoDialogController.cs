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

        protected override void RegisterEvents()
        {
            _yesNoDialogEventSO.ShowEvent += ShowDialogRequested;
        }

        private void ShowDialogRequested()
        {
            LoadAssetDialog();
        }

        protected override void UnregisterEvents()
        {
            _yesNoDialogEventSO.ShowEvent -= ShowDialogRequested;
        }

        protected override void SetupDialog(UIYesNoDialog dialog)
        {
            dialog.Show();
        }
    }
}
