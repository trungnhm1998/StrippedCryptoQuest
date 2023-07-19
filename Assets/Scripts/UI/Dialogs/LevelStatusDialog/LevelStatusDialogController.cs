using System.Collections;
using System.Collections.Generic;
using CryptoQuest.Events.UI;
using CryptoQuest.Gameplay.Quest;
using UnityEngine;

namespace CryptoQuest.UI.Dialogs.LevelStatusDialog
{
    public class LevelStatusController : AbstractDialogController<UILevelStatusDialog>
    {
        [SerializeField] private LevelStatusDialogEventChannelSO _rewardDialogEvent;
        private LevelStatusDialogData _levelStatusDialogData;

        protected override void RegisterEvents()
        {
            _rewardDialogEvent.ShowEvent += ShowDialog;
        }

        protected override void UnregisterEvents()
        {
            _rewardDialogEvent.ShowEvent -= ShowDialog;
        }

        private void ShowDialog(LevelStatusDialogData levelStatusDialogData)
        {
            _levelStatusDialogData = levelStatusDialogData;
            LoadAssetDialog();
        }

        protected override void SetupDialog(UILevelStatusDialog dialog)
        {
            dialog
                .SetDialog(_levelStatusDialogData)
                .Show();
        }
    }
}
